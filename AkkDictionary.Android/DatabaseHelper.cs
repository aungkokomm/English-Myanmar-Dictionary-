using Android.Content;
using SQLite;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AkkDictionary.Android;

public class HeadwordResult
{
    public string DisplayHeadword { get; set; } = string.Empty;
    public string Pos             { get; set; } = string.Empty;
    public int    Senses          { get; set; }
    public string Preview         { get; set; } = string.Empty;
}

public class DatabaseHelper
{
    private static DatabaseHelper? _instance;
    private readonly string _dbPath;
    private const string DbName = "dictionary.db";

    private static readonly Regex NonAlnum = new(@"[^A-Za-z0-9က-႟]+", RegexOptions.Compiled);
    private static string ToSearchKey(string w)
    {
        w = (w ?? string.Empty).ToLowerInvariant();
        w = NonAlnum.Replace(w, " ");
        return w.Trim();
    }

    public static DatabaseHelper GetInstance(Context context)
    {
        if (_instance == null)
            _instance = new DatabaseHelper(context.ApplicationContext!);
        return _instance;
    }

    private DatabaseHelper(Context context)
    {
        _dbPath = Path.Combine(context.FilesDir!.AbsolutePath, DbName);
        if (!File.Exists(_dbPath))
            CopyFromAssets(context);
    }

    private void CopyFromAssets(Context context)
    {
        using var input  = context.Assets!.Open(DbName);
        using var output = File.Create(_dbPath);
        input.CopyTo(output);
    }

    public List<HeadwordResult> SearchHeadwords(string query, bool reverseSearch, int limit = 400)
    {
        using var db = new SQLiteConnection(_dbPath, SQLiteOpenFlags.ReadOnly | SQLiteOpenFlags.FullMutex);

        if (reverseSearch)
        {
            // Outer query adds a definition preview via correlated subquery
            return db.Query<HeadwordResult>(@"
                SELECT display_headword AS DisplayHeadword, pos AS Pos, COUNT(*) AS Senses,
                  (SELECT definition FROM entries WHERE display_headword = e.display_headword ORDER BY ROWID LIMIT 1) AS Preview
                FROM entries e WHERE definition LIKE ?
                GROUP BY display_headword, pos ORDER BY display_headword LIMIT ?",
                "%" + query + "%", limit);
        }

        string key = ToSearchKey(query);
        return db.Query<HeadwordResult>(@"
            SELECT sub.DisplayHeadword, sub.Pos, sub.Senses,
              (SELECT definition FROM entries WHERE display_headword = sub.DisplayHeadword ORDER BY ROWID LIMIT 1) AS Preview
            FROM (
              SELECT display_headword AS DisplayHeadword, pos AS Pos, COUNT(*) AS Senses, 1 AS rank
                FROM entries WHERE search_key = ?
                GROUP BY display_headword, pos
              UNION ALL
              SELECT display_headword AS DisplayHeadword, pos AS Pos, COUNT(*) AS Senses, 2 AS rank
                FROM entries WHERE search_key LIKE ? AND search_key <> ?
                GROUP BY display_headword, pos
              UNION ALL
              SELECT display_headword AS DisplayHeadword, pos AS Pos, COUNT(*) AS Senses, 3 AS rank
                FROM entries WHERE search_key LIKE ? AND search_key NOT LIKE ?
                GROUP BY display_headword, pos
            ) sub
            ORDER BY sub.rank, sub.DisplayHeadword LIMIT ?",
            key,
            key + "%", key,
            "%" + key + "%", key + "%",
            limit);
    }

    public List<string> GetDefinitions(string headword, string pos)
    {
        using var db = new SQLiteConnection(_dbPath, SQLiteOpenFlags.ReadOnly | SQLiteOpenFlags.FullMutex);
        var rows = db.Query<DefinitionRow>(
            "SELECT definition AS Definition FROM entries " +
            "WHERE display_headword = ? AND (pos = ? OR (? = '' AND (pos IS NULL OR pos = ''))) " +
            "ORDER BY ROWID LIMIT 500",
            headword, pos, pos);
        return rows.Select(r => r.Definition).ToList();
    }

    private class DefinitionRow
    {
        public string Definition { get; set; } = string.Empty;
    }
}
