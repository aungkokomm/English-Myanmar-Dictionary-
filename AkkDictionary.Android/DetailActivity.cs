using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace AkkDictionary.Android;

[Activity(Label = "@string/definitions", Theme = "@android:style/Theme.Material.Light.NoActionBar")]
public class DetailActivity : Activity
{
    public const string ExtraHeadword = "headword";
    public const string ExtraPos      = "pos";

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.activity_detail);

        var headword = Intent?.GetStringExtra(ExtraHeadword) ?? string.Empty;
        var pos      = Intent?.GetStringExtra(ExtraPos)      ?? string.Empty;

        FindViewById<TextView>(Resource.Id.headwordText)!.Text = headword;
        FindViewById<Button>(Resource.Id.backButton)!.Click   += (_, _) => Finish();

        // POS badge
        var posView = FindViewById<TextView>(Resource.Id.posText)!;
        if (!string.IsNullOrWhiteSpace(pos))
        {
            posView.Text       = pos;
            posView.Visibility = ViewStates.Visible;
            var bg = new GradientDrawable();
            bg.SetShape(ShapeType.Rectangle);
            bg.SetCornerRadius(5f);
            bg.SetColor(PosColor(pos));
            posView.Background = bg;
        }

        var definitions  = DatabaseHelper.GetInstance(this).GetDefinitions(headword, pos);
        var sensesCount  = FindViewById<TextView>(Resource.Id.sensesCount)!;
        sensesCount.Text = definitions.Count == 1 ? "1 definition" : $"{definitions.Count} definitions";

        FindViewById<ListView>(Resource.Id.definitionsList)!.Adapter =
            new DefinitionAdapter(this, definitions);
    }

    private static Color PosColor(string pos)
    {
        var p = pos.ToLowerInvariant();
        if (p.StartsWith("n"))    return Color.ParseColor("#388E3C");
        if (p.StartsWith("v"))    return Color.ParseColor("#D32F2F");
        if (p.StartsWith("adj")) return Color.ParseColor("#7B1FA2");
        if (p.StartsWith("adv")) return Color.ParseColor("#00838F");
        if (p.StartsWith("pro")) return Color.ParseColor("#E65100");
        if (p.StartsWith("pre")) return Color.ParseColor("#455A64");
        if (p.StartsWith("con")) return Color.ParseColor("#5D4037");
        if (p.StartsWith("int")) return Color.ParseColor("#C62828");
        return Color.ParseColor("#757575");
    }

    private sealed class DefinitionAdapter : BaseAdapter<string>
    {
        private readonly List<string> _items;
        private readonly Activity     _ctx;

        public DefinitionAdapter(Activity ctx, List<string> items) { _ctx = ctx; _items = items; }

        public override int    Count                   => _items.Count;
        public override string this[int position]      => _items[position];
        public override long   GetItemId(int position) => position;

        public override View GetView(int position, View? convertView, ViewGroup? parent)
        {
            var view   = convertView ?? _ctx.LayoutInflater.Inflate(Resource.Layout.item_definition, parent, false)!;
            var num    = view.FindViewById<TextView>(Resource.Id.defNumber)!;
            var text   = view.FindViewById<TextView>(Resource.Id.defText)!;
            num.Text   = $"{position + 1}.";
            text.Text  = _items[position];
            return view;
        }
    }
}
