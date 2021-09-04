
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using ForConsumption.Common;

using Microcharts;

using SkiaSharp;

using Xamarin.Forms;

namespace ForConsumption.Converters
{
    public class StatisticsItemConverter : IValueConverter
    {
        public static readonly List<string> Colors = new List<string>()
        {
          "#9dd3a8","#d9d9f3","#f4f0e6","#ffeead","#d9534f","#ffad60","#de4307","#8bc24c",
          "#ffb6b9","#8ac6d1","#fff1ac","#f9bcdd","#b689b0","#525252","#1ee3cf","#6b48ff","#004d61",
          "#ff502f","#e41749","#ff8a5c","#001871","#ff585d","#41b6e6","#f6003c","#260033","#1f640a",

          "#3b9a9c","#c50d66","#f07810","#2d2d2d","#a39e9e","#a39391","#e79686","#f06966","#6abe83","#96ceb4",
        };
        private static SKTypeface sKTypeface;

        private static SKColor GetColor(int index)
        {
            return SKColor.Parse(Colors[index]);
        }

        private static SKTypeface SKTypeface
        {
            get
            {
                if (sKTypeface is null)
                {
                    sKTypeface = SKFontManager.Default.MatchCharacter('汉');
                }
                return sKTypeface;
            }
        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not ICollection<ItemsDisplay> list)
            {
                return null;
            }

            if (list.Count == 0)
            {
                return null;
            }

            int EntryColorIndex = 0;
            int SerieColorIndex = 0;

            int type = (int)System.Convert.ChangeType(parameter, typeof(int));
            List<ChartEntry> charts = new List<ChartEntry>();
            //Dictionary<string, ConsumptionItem[]> dict = list.GroupBy(i => i.Category).ToDictionary(i => i.Key, i => i.ToArray());

            decimal allTotal = list.Sum(i => i.TotalMoney);


            foreach (ItemsDisplay item in list)
            {
                decimal total = item.Sum(ii => ii.Money);

                ChartEntry entry = new ChartEntry((float)total)
                {
                    Label = $"{item.Header}  { total / allTotal * 100:F2}%",
                    // ValueLabel = $"{total:F2}",
                    Color = GetColor(EntryColorIndex)
                };
                EntryColorIndex++;
                charts.Add(entry);

            };

            switch (type)
            {
                //饼图
                case 1:
                    DonutChart chart1 = new DonutChart()
                    {
                        GraphPosition = GraphPosition.AutoFill,
                        LabelMode = LabelMode.RightOnly
                    };
                    //chart1.HoleRadius = 0;
                    chart1.Entries = charts;
                    return Attach(chart1);

                //柱形图
                case 2:
                    LegacyBarChart chart2 = new LegacyBarChart()
                    {
                        Entries = charts,
                        LabelOrientation = Orientation.Horizontal,
                        ValueLabelOrientation = Orientation.Horizontal,
                        //ValueLabelOption = ValueLabelOption.TopOfElement,
                        //Series = new[] {new ChartSerie
                        //{
                        //    Name = Guid.NewGuid().ToString(),
                        //    Color = GetColor(SerieColorIndex),
                        //    Entries = charts
                        //}}
                    };
                    return Attach(chart2);
                //线形图
                case 3:
                    ChartSerie def = new ChartSerie
                    {
                        Name = Guid.NewGuid().ToString(),
                        Color = GetColor(SerieColorIndex),
                    };
                    List<ChartEntry> ll = new List<ChartEntry>();
                    int index = 1;
                    for (int i = 0; i < 3; i++)
                    {
                        foreach (ChartEntry item in charts)
                        {
                            ChartEntry entry = new ChartEntry(item.Value)
                            {
                                Label = $"{index}月",
                                ValueLabel = item.ValueLabel,
                                Color = item.Color
                            };
                            index++;
                            ll.Add(entry);
                        }
                    }

                    def.Entries = ll;

                    LineChart chart3 = new LineChart
                    {
                        Series = new[] { def },
                    };
                    Attach(chart3);

                    chart3.PointSize = 1;
                    return (chart3);
                //散点图
                case 4:
                    PointChart chart4 = new PointChart
                    {
                        Series = new[] {new ChartSerie
                    {
                        Name = Guid.NewGuid().ToString(),
                        Color = GetColor(SerieColorIndex),
                        Entries = charts
                    } }
                    };
                    return Attach(chart4);
                //雷达图
                case 5:
                    RadarChart chart5 = new RadarChart
                    {
                        Entries = charts
                    };
                    return Attach(chart5);
                default:
                    break;
            }

            return null;


            TChart Attach<TChart>(TChart chart) where TChart : Chart
            {
                if (chart is AxisBasedChart chart1)
                {
                    chart1.LabelOrientation = Orientation.Horizontal;
                    chart1.ValueLabelOrientation = Orientation.Horizontal;
                    chart1.LegendOption = SeriesLegendOption.None;
                    chart1.ValueLabelOption = ValueLabelOption.OverElement;
                    chart1.ValueLabelTextSize = 30;
                    chart1.SerieLabelTextSize = 36;

                }
                chart.LabelTextSize = 25;
                chart.Typeface = SKTypeface;
                return chart;
            }

        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
