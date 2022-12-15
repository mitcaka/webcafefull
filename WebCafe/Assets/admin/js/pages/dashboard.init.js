var options = {
    chart: {
        height: 359, type: "bar", stacked: !0,
        toolbar: { show: !1 }, zoom: { enabled: !0 }
    },
    plotOptions: {
        bar: {
            horizontal: !1, columnWidth: "15%",
            endingShape: "rounded"
        }
    },
    dataLabels: { enabled: !1 },
    series: [
        { name: "Tiền nhập", data: [44000, 55000, 41000, 67000, 22000, 43000, 36000, 52000, 24000, 18000, 36000, 48000] },
        { name: "Tiền lãi", data: [13000, 23000, 20000, 8000, 13000, 27000, 18000, 22000, 10000, 16000, 24000, 22000] }

    ],
    xaxis: {
        categories: ["Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"]
    },
    colors: ["#556ee6", "#f1b44c", "#34c38f"], legend: { position: "bottom" },
    fill: { opacity: 1 }
},
    chart = new ApexCharts(document.querySelector("#stacked-column-chart"), options); chart.render();
options = {
    chart: { height: 180, type: "radialBar", offsetY: -10 },
    plotOptions: {
        radialBar: {
            startAngle: -135, endAngle: 135,
            dataLabels: {
                name: { fontSize: "13px", color: void 0, offsetY: 60 },
                value: { offsetY: 22, fontSize: "16px", color: void 0, formatter: function (e) { return e + "%" } }
            }
        }
    },
    colors: ["#556ee6"], fill: {
        type: "gradient", gradient: {
            shade: "dark", shadeIntensity: .15,
            inverseColors: !1, opacityFrom: 1, opacityTo: 1, stops: [0, 50, 65, 91]
        }
    }, stroke: { dashArray: 4 },
    series: [67], labels: ["Series A"]
}; (chart = new ApexCharts(document.querySelector("#radialBar-chart"), options)).render();