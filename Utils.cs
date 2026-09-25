using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;
using ExcelDataReader;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AkkDictionaryApp
{
    public static class Utils
    {
        private static readonly Regex ZeroWidth = new Regex("[​-‍﻿]", RegexOptions.Compiled);
        private static readonly Regex NonAlnumForSearch = new Regex("[^A-Za-z0-9က-႟]+", RegexOptions.Compiled);
        private static readonly Regex SeeAlso = new Regex(@"(?i)(see also|see)\s+([A-Za-z0-9\-\s'_/]+)", RegexOptions.Compiled);

        public static IEnumerable<(int start, int length)> FindSeeAlso(string s)
        {
            var input = s ?? string.Empty;
            var matches = SeeAlso.Matches(input);
            foreach (System.Text.RegularExpressions.Match m in matches)
            {
                var g = m.Groups.Count > 2 ? m.Groups[2] : m.Groups[0];
                yield return (g.Index, g.Length);
            }
        }

        public static string NFC(string s) => (s ?? string.Empty).Normalize(NormalizationForm.FormC);
        public static string CollapseSpaces(string s) => System.Text.RegularExpressions.Regex.Replace(s ?? string.Empty, @"\s+", " ").Trim();
        public static string RemoveZeroWidth(string s) => ZeroWidth.Replace(s ?? string.Empty, string.Empty);

        public static string CleanText(object value)
        { if (value==null) return string.Empty; var s=Convert.ToString(value) ?? string.Empty; if (string.Equals(s,"nan",StringComparison.OrdinalIgnoreCase)) return string.Empty; return CollapseSpaces(RemoveZeroWidth(NFC(s))); }

        public static string DisplayHeadword(string w)
        { w=(w ?? string.Empty).Replace(@"\/","/").Replace(@"\-","-").Replace(@"", string.Empty); return CollapseSpaces(w); }

        public static string SearchKey(string w)
        { w=(w ?? string.Empty).ToLowerInvariant(); w=NonAlnumForSearch.Replace(w, " "); return CollapseSpaces(w); }

        /// <summary>Levenshtein edit distance between two strings.</summary>
        public static int EditDistance(string a, string b)
        {
            if (string.IsNullOrEmpty(a)) return b?.Length ?? 0;
            if (string.IsNullOrEmpty(b)) return a.Length;
            var dp = new int[a.Length + 1, b.Length + 1];
            for (int i = 0; i <= a.Length; i++) dp[i, 0] = i;
            for (int j = 0; j <= b.Length; j++) dp[0, j] = j;
            for (int i = 1; i <= a.Length; i++)
                for (int j = 1; j <= b.Length; j++)
                    dp[i, j] = a[i - 1] == b[j - 1] ? dp[i - 1, j - 1]
                                : 1 + Math.Min(dp[i - 1, j - 1], Math.Min(dp[i - 1, j], dp[i, j - 1]));
            return dp[a.Length, b.Length];
        }

        /// <summary>Find headwords within maxDistance edits of query. Candidates are filtered by first letter for speed.</summary>
        public static async Task<List<string>> FuzzySearchAsync(string dbPath, string query, int maxDistance = 2, int limit = 10)
        {
            var key = SearchKey(query);
            if (key.Length < 3) return new List<string>();
            if (!File.Exists(dbPath)) return new List<string>();

            var results = new List<(string display, int dist)>();
            using var conn = new SqliteConnection(new SqliteConnectionStringBuilder { DataSource = dbPath }.ToString());
            await conn.OpenAsync();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT DISTINCT display_headword, search_key FROM entries
                                WHERE search_key LIKE @prefix
                                ORDER BY search_key LIMIT 3000";
            cmd.Parameters.AddWithValue("@prefix", key[0] + "%");
            using var rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                var sk = rdr.GetString(1);
                if (Math.Abs(sk.Length - key.Length) > maxDistance) continue;
                var dist = EditDistance(key, sk);
                if (dist > 0 && dist <= maxDistance)
                    results.Add((rdr.GetString(0), dist));
            }
            return results.OrderBy(r => r.dist).ThenBy(r => r.display).Take(limit).Select(r => r.display).ToList();
        }

        public static string NormalizePos(string p)
        {
            p = CleanText(p);
            if (string.IsNullOrEmpty(p)) return string.Empty;
            var tmp = System.Text.RegularExpressions.Regex.Replace(p, @"[\s]*[,/][\s]*", ",");
            var parts = tmp.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                           .Select(x => x.Replace("adj.", "adj").Replace("adv.", "adv"));
            return string.Join(';', parts);
        }

        public static void RebuildDatabaseFromExcel(string excelPath, string dbPath)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using var stream = File.Open(excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream);
            var ds = reader.AsDataSet(new ExcelDataReader.ExcelDataSetConfiguration{ ConfigureDataTable = _ => new ExcelDataReader.ExcelDataTableConfiguration{ UseHeaderRow = true } });
            if (ds.Tables.Count==0) throw new Exception("No sheets found.");
            var table = ds.Tables[0]; var cols = table.Columns.Cast<System.Data.DataColumn>().Select(c => c.ColumnName).ToList();
            if (!cols.Contains("Word") || !cols.Contains("state") || !cols.Contains("def")) throw new Exception("The first sheet must have columns: Word, state, def (ID optional)");
            var rows = new List<(string head,string pos,string def)>();
            foreach (DataRow r in table.Rows)
            { var head=CleanText(r["Word"]); var pos=NormalizePos(Convert.ToString(r["state"]) ?? ""); var def=CleanText(r["def"]); if (string.IsNullOrWhiteSpace(head)||string.IsNullOrWhiteSpace(def)) continue; rows.Add((head,pos,def)); }
            WriteRowsToDb(rows, dbPath);
        }

        public static void RebuildDatabaseFromSqlite(string sourceDbPath, string table, string headCol, string defCol, string? posCol, string targetDbPath)
        {
            using var src = new SqliteConnection(new SqliteConnectionStringBuilder{ DataSource = sourceDbPath }.ToString());
            src.Open();
            using var cur = src.CreateCommand();
            cur.CommandText = $"SELECT [{headCol}], {(string.IsNullOrWhiteSpace(posCol)?"NULL AS pos":$"[{posCol}] AS pos")}, [{defCol}] FROM [{table}]";
            using var rdr = cur.ExecuteReader();
            var rows = new List<(string head,string pos,string def)>();
            while (rdr.Read())
            {
                var head=CleanText(rdr[0]); var pos=NormalizePos(Convert.ToString(rdr[1]) ?? ""); var def=CleanText(rdr[2]);
                if (string.IsNullOrWhiteSpace(head)||string.IsNullOrWhiteSpace(def)) continue; rows.Add((head,pos,def));
            }
            WriteRowsToDb(rows, targetDbPath);
        }

        private static void WriteRowsToDb(IEnumerable<(string head,string pos,string def)> rows, string dbPath)
        {
            if (File.Exists(dbPath)) File.Delete(dbPath);
            using var dst = new SqliteConnection(new SqliteConnectionStringBuilder{ DataSource = dbPath }.ToString());
            dst.Open();
            using (var cmd = dst.CreateCommand())
            { cmd.CommandText = @"CREATE TABLE entries (
 id INTEGER PRIMARY KEY AUTOINCREMENT,
 headword TEXT NOT NULL,
 pos TEXT,
 definition TEXT NOT NULL,
 display_headword TEXT NOT NULL,
 search_key TEXT NOT NULL
);"; cmd.ExecuteNonQuery(); }
            using (var idx1 = dst.CreateCommand()) { idx1.CommandText = "CREATE INDEX idx_entries_headword ON entries(display_headword)"; idx1.ExecuteNonQuery(); }
            using (var idx2 = dst.CreateCommand()) { idx2.CommandText = "CREATE INDEX idx_entries_search_key ON entries(search_key)"; idx2.ExecuteNonQuery(); }
            using (var idx3 = dst.CreateCommand()) { idx3.CommandText = "CREATE INDEX idx_entries_headword_pos ON entries(display_headword, pos)"; idx3.ExecuteNonQuery(); }

            using var tx = dst.BeginTransaction();
            using var ins = dst.CreateCommand();
            ins.CommandText = "INSERT INTO entries(headword,pos,definition,display_headword,search_key) VALUES (@h,@p,@d,@dh,@k)";
            var ph=ins.CreateParameter(); ph.ParameterName="@h"; ins.Parameters.Add(ph);
            var pp=ins.CreateParameter(); pp.ParameterName="@p"; ins.Parameters.Add(pp);
            var pd=ins.CreateParameter(); pd.ParameterName="@d"; ins.Parameters.Add(pd);
            var pdh=ins.CreateParameter(); pdh.ParameterName="@dh"; ins.Parameters.Add(pdh);
            var pk=ins.CreateParameter(); pk.ParameterName="@k"; ins.Parameters.Add(pk);

            foreach (var (head,pos,def) in rows.Distinct())
            {
                var disp = DisplayHeadword(head); var key = SearchKey(disp);
                ph.Value=head; pp.Value=pos; pd.Value=def; pdh.Value=disp; pk.Value=key; ins.ExecuteNonQuery();
            }
            tx.Commit();
        }
    }

    public class AppSettings
    {
        public bool EnableSuggestions { get; set; } = true;
        public bool DefaultReverseSearch { get; set; } = false;
        public string UiFontFamily { get; set; } = "Myanmar Text";
        public double FontScale { get; set; } = 1.0;
        public bool RememberWindow { get; set; } = true;
        public double? WindowWidth { get; set; }
        public double? WindowHeight { get; set; }
        public List<string> SearchHistory { get; set; } = new();
        public bool DarkMode { get; set; } = false;
        public List<string> Bookmarks { get; set; } = new();
        public double ListColumnWidth { get; set; } = 420;

        public void AddToHistory(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return;
            SearchHistory.Remove(query);
            SearchHistory.Insert(0, query);
            if (SearchHistory.Count > 20)
                SearchHistory.RemoveRange(20, SearchHistory.Count - 20);
        }

        public static AppSettings Load(string path)
        { try{ if (File.Exists(path)){ var json=System.Text.Json.JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(path)); if (json!=null) return json; } } catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[Settings] Load failed: {ex.Message}"); } return new AppSettings(); }
        public static void Save(AppSettings s, string path)
        { try{ var json=System.Text.Json.JsonSerializer.Serialize(s, new System.Text.Json.JsonSerializerOptions{ WriteIndented=true }); File.WriteAllText(path, json);} catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[Settings] Save failed: {ex.Message}"); } }
    }
}
