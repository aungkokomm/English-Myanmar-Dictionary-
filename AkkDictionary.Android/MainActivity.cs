using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AkkDictionary.Android;

[Activity(Label = "@string/app_name", MainLauncher = true, Theme = "@android:style/Theme.Material.Light.NoActionBar")]
public class MainActivity : Activity
{
    private DatabaseHelper?          _db;
    private readonly List<HeadwordResult> _results = new();
    private ResultAdapter?           _adapter;
    private CancellationTokenSource? _searchCts;
    private string                   _lastQuery = string.Empty;

    private EditText?    _searchInput;
    private CheckBox?    _reverseToggle;
    private ProgressBar? _progressBar;
    private ListView?    _resultsView;
    private TextView?    _statusText;
    private TextView?    _queryLabel;
    private ViewGroup?   _emptyState;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.activity_main);

        _searchInput   = FindViewById<EditText>(Resource.Id.searchInput)!;
        _reverseToggle = FindViewById<CheckBox>(Resource.Id.reverseSearchToggle)!;
        _progressBar   = FindViewById<ProgressBar>(Resource.Id.progressBar)!;
        _resultsView   = FindViewById<ListView>(Resource.Id.resultsView)!;
        _statusText    = FindViewById<TextView>(Resource.Id.statusText)!;
        _queryLabel    = FindViewById<TextView>(Resource.Id.queryLabel)!;
        _emptyState    = FindViewById<ViewGroup>(Resource.Id.emptyState)!;

        _adapter = new ResultAdapter(this, _results);
        _resultsView.Adapter = _adapter;

        _resultsView.ItemClick += (_, e) =>
        {
            if (e.Position < 0 || e.Position >= _results.Count) return;
            var row    = _results[e.Position];
            var intent = new Intent(this, typeof(DetailActivity));
            intent.PutExtra(DetailActivity.ExtraHeadword, row.DisplayHeadword);
            intent.PutExtra(DetailActivity.ExtraPos,      row.Pos ?? string.Empty);
            StartActivity(intent);
        };

        _searchInput.TextChanged                  += (_, _) => TriggerSearch();
        FindViewById<Button>(Resource.Id.searchButton)!.Click += (_, _) => TriggerSearch(immediate: true);
        _searchInput.EditorAction += (_, e) => { if (e.ActionId == ImeAction.Search) TriggerSearch(immediate: true); };
        _reverseToggle.CheckedChange += (_, _) => TriggerSearch(immediate: true);

        // Init DB in background
        _progressBar.Visibility = ViewStates.Visible;
        _statusText.Text        = GetString(Resource.String.initializing);
        Task.Run(() => { _db = DatabaseHelper.GetInstance(this); })
            .ContinueWith(_ => RunOnUiThread(() =>
            {
                _progressBar.Visibility = ViewStates.Gone;
                _statusText.Text        = GetString(Resource.String.tap_hint);
            }));
    }

    private void TriggerSearch(bool immediate = false)
    {
        _searchCts?.Cancel();
        _searchCts = new CancellationTokenSource();
        var token  = _searchCts.Token;
        Task.Delay(immediate ? 0 : 300, token)
            .ContinueWith(t => { if (!t.IsCanceled) RunOnUiThread(() => DoSearch(token)); },
                TaskScheduler.Default);
    }

    private void DoSearch(CancellationToken token)
    {
        if (_db == null) return;
        var query   = _searchInput?.Text?.Trim() ?? string.Empty;
        var reverse = _reverseToggle?.Checked ?? false;
        _lastQuery  = query;

        _progressBar!.Visibility = ViewStates.Visible;

        Task.Run(() => _db.SearchHeadwords(query, reverse), token)
            .ContinueWith(t =>
            {
                if (t.IsCanceled || t.IsFaulted) return;
                RunOnUiThread(() =>
                {
                    _progressBar.Visibility = ViewStates.Gone;
                    if (token.IsCancellationRequested) return;

                    _results.Clear();
                    _results.AddRange(t.Result);
                    _adapter!.NotifyDataSetChanged();

                    bool hasResults = _results.Count > 0;
                    _emptyState!.Visibility  = hasResults ? ViewStates.Gone    : ViewStates.Visible;
                    _resultsView!.Visibility = hasResults ? ViewStates.Visible : ViewStates.Gone;

                    if (hasResults)
                    {
                        var label = string.IsNullOrWhiteSpace(query)
                            ? $"{_results.Count} entries"
                            : $"{_results.Count} result{(_results.Count == 1 ? "" : "s")} for \"{query}\"";
                        _queryLabel!.Text       = label;
                        _queryLabel.Visibility  = ViewStates.Visible;
                    }
                    else
                    {
                        _queryLabel!.Visibility = ViewStates.Gone;
                        _statusText!.Text       = string.IsNullOrWhiteSpace(query)
                            ? GetString(Resource.String.tap_hint)
                            : GetString(Resource.String.no_results);
                    }
                });
            }, TaskScheduler.Default);
    }

    // ── Adapter ──────────────────────────────────────────────────────────────

    private sealed class ResultAdapter : BaseAdapter<HeadwordResult>
    {
        private readonly List<HeadwordResult> _items;
        private readonly Activity             _ctx;

        public ResultAdapter(Activity ctx, List<HeadwordResult> items) { _ctx = ctx; _items = items; }

        public override int            Count                   => _items.Count;
        public override HeadwordResult this[int position]      => _items[position];
        public override long           GetItemId(int position) => position;

        public override View GetView(int position, View? convertView, ViewGroup? parent)
        {
            var view = convertView ?? _ctx.LayoutInflater.Inflate(Resource.Layout.item_headword, parent, false)!;

            var row     = _items[position];
            var hw      = view.FindViewById<TextView>(Resource.Id.itemHeadword)!;
            var posView = view.FindViewById<TextView>(Resource.Id.itemPos)!;
            var senses  = view.FindViewById<TextView>(Resource.Id.itemSenses)!;
            var preview = view.FindViewById<TextView>(Resource.Id.itemPreview)!;

            hw.Text      = row.DisplayHeadword;
            senses.Text  = row.Senses > 1 ? $"{row.Senses} defs" : "1 def";
            preview.Text = row.Preview;

            // POS badge
            var posLabel = (row.Pos ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(posLabel))
            {
                posView.Visibility = ViewStates.Gone;
            }
            else
            {
                posView.Visibility = ViewStates.Visible;
                posView.Text       = posLabel.Length > 10 ? posLabel[..10] : posLabel;
                var bg = new GradientDrawable();
                bg.SetShape(ShapeType.Rectangle);
                bg.SetCornerRadius(5f);
                bg.SetColor(PosColor(posLabel));
                posView.Background = bg;
            }

            return view;
        }

        private static Color PosColor(string pos)
        {
            var p = pos.ToLowerInvariant();
            if (p.StartsWith("n"))    return Color.ParseColor("#388E3C"); // green  – noun
            if (p.StartsWith("v"))    return Color.ParseColor("#D32F2F"); // red    – verb
            if (p.StartsWith("adj")) return Color.ParseColor("#7B1FA2"); // purple – adjective
            if (p.StartsWith("adv")) return Color.ParseColor("#00838F"); // teal   – adverb
            if (p.StartsWith("pro")) return Color.ParseColor("#E65100"); // orange – pronoun/proper
            if (p.StartsWith("pre")) return Color.ParseColor("#455A64"); // blue-grey – preposition
            if (p.StartsWith("con")) return Color.ParseColor("#5D4037"); // brown  – conjunction/contraction
            if (p.StartsWith("int")) return Color.ParseColor("#C62828"); // dark red – interjection
            return Color.ParseColor("#757575");                            // grey   – other
        }
    }
}
