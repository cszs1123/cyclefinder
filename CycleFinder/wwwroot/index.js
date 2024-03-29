const log = console.log;

const chartProterties = {
    width:1800,
    height:900,
    timeScale:{
        timeVisible:true,
        secondsVisible:false,
    }
}

const domElement = document.getElementById('tvchart');
const chart = LightweightCharts.createChart(domElement, chartProterties);
const candleSeries = chart.addCandlestickSeries();

function getPlanetaryLines(planet, currentPrice, from) {

    fetch(`https://localhost:5001/api/PlanetaryLines/GetPlanetaryLines?planet=${planet}&currentPrice=${currentPrice}&from=${from}`)
        .then(res => res.json())
        .then(data => {
            console.log(JSON.stringify(data, null, '\t'));

            var i;
            for (i = 0; i < data.length; i++) {
                const lineSeries = chart.addLineSeries({
                    color: data[i].color,
                    lineWidth: 2,
                    priceLineVisible: false
                });
                lineSeries.setData(data[i].lineValues);
            }
        })
        .catch(err => log(err))
}

function getAspects(p1, p2, from) {

}

chart.applyOptions({
    crosshair: {
        mode: 0,
    },
});


//Get all data
fetch('https://localhost:5001/api/CandleStick/GetAllData?symbol=BTCUSDT')
    .then(res => res.json())
    .then(data => {
        
        candleSeries.setData(data);

        //get planetary lines
        getPlanetaryLines('ur', data[data.length - 2].open, data[0].time)
        //getPlanetaryLines('sa', data[data.length - 2].open, data[0].time)
       // getPlanetaryLines('ju', data[data.length - 2].open, data[0].time)
        //getPlanetaryLines('pl', data[data.length - 2].open, data[0].time)

        //Get w24 lines

        var maxValue = data[data.length - 1].high * 2
        fetch(`https://localhost:5001/api/PriceLevels/GetW24PriceLevels?maxValue=${maxValue}&increment=100`)
            .then(res => res.json())
            .then(data => {
                console.log(JSON.stringify(data, null, '\t'));

                var i;
                for (i = 0; i < data.length; i++) {
                    candleSeries.createPriceLine({
                        price: data[i].price,
                        color: data[i].lineColor,
                        lineWidth: data[i].lineWidth,
                        lineStyle: LightweightCharts.LineStyle.Dashed
                    });
                }
            })
            .catch(err => log(err))

        //Get aspects
        fetch(`https://localhost:5001/api/CandleStickMarker/GetAspects?from=${data[0].time}&planet=me,sa`)
            .then(res => res.json())
            .then(data => {
                console.log(JSON.stringify(data, null, '\t'));

                const markers = data.filter(l => !l.isInTheFuture).map(d => {
                    return {
                        time: d.time,
                        position: d.position,
                        color: d.color,
                        shape: d.shape,
                        text: d.text,
                    }
                });

                candleSeries.setMarkers(markers);
            })
            .catch(err => log(err))
    })
    .catch(err => log(err))




