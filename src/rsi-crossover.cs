using cAlgo.API;
using cAlgo.API.Internals;
using cAlgo.API.Indicators;
using cAlgo.Indicators;
using System;

namespace cAlgo.Indicators
{

    [Indicator(IsOverlay = false, TimeZone = TimeZones.UTC, AutoRescale = false, AccessRights = AccessRights.None)]
    public class RSI : Indicator
    {

        private RelativeStrengthIndex rsi1, rsi2;
        private MarketSeries series1, series2;
        private Symbol symbol1, symbol2;

        [Parameter(DefaultValue = "XAUUSD")]
        public string Symbol1 { get; set; }

        [Parameter(DefaultValue = "US 500")]
        public string Symbol2 { get; set; }


        [Output("RSI Symbol 1", Color = Colors.Yellow)]
        public IndicatorDataSeries Result1 { get; set; }

        [Output("RSI Symbol 2", Color = Colors.Red)]
        public IndicatorDataSeries Result2 { get; set; }


        [Parameter("Typical(OHC/3)", DefaultValue = true)]
        public bool is_typic { get; set; }
        [Parameter("WClose (OHCC/4)", DefaultValue = false)]
        public bool is_wclose { get; set; }
        [Parameter("Median (OH/2)", DefaultValue = false)]
        public bool is_median { get; set; }
        [Parameter("Period:", DefaultValue = 14)]
        public int Period { get; set; }

        [Output("level80", Color = Colors.Red, LineStyle = LineStyle.Dots, PlotType = PlotType.Line, Thickness = 1)]
        public IndicatorDataSeries level80 { get; set; }
        [Output("level20", Color = Colors.Green, LineStyle = LineStyle.Dots, PlotType = PlotType.Line, Thickness = 1)]
        public IndicatorDataSeries level20 { get; set; }

        public RelativeStrengthIndex rsi;

        protected override void Initialize()
        {
            symbol1 = MarketData.GetSymbol(Symbol1);
            symbol2 = MarketData.GetSymbol(Symbol2);

            series1 = MarketData.GetSeries(symbol1, TimeFrame);
            series2 = MarketData.GetSeries(symbol2, TimeFrame);

            rsi1 = Indicators.RelativeStrengthIndex(series1.Close, Period);
            rsi2 = Indicators.RelativeStrengthIndex(series2.Close, Period);

        }

        private int GetIndexByDate(MarketSeries series, DateTime time)
        {
            for (int i = series.Close.Count - 1; i > 0; i--)
            {
                if (time == series.OpenTime[i])
                    return i;
            }
            return -1;
        }

        public override void Calculate(int index)
        {
            
            var index1 = GetIndexByDate(series1, MarketSeries.OpenTime[index]);
            if (index1 != -1)
                Result1[index] = rsi1.Result[index1];

            var index2 = GetIndexByDate(series2, MarketSeries.OpenTime[index]);
            if (index2 != -1)
                Result2[index] = rsi2.Result[index2];

            level80[index] = 80;
            level20[index] = 20;
        }
    }
}

