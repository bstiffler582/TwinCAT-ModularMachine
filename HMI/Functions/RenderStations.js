// Keep these lines for a best effort IntelliSense of Visual Studio 2017 and higher.
/// <reference path="./../../Packages/Beckhoff.TwinCAT.HMI.Framework.14.3.360/runtimes/native1.12-tchmi/TcHmi.d.ts" />

(function (/** @type {globalThis.TcHmi} */ TcHmi) {
    var Functions;
    (function (/** @type {globalThis.TcHmi.Functions} */ Functions) {
        var HMI;
        (function (HMI) {
            function RenderStations(stationsA, stationsB, grid) {

                stationsA.forEach((station, i) => {
                    if (station.nIndex !== 0 && station.bEnabled) {
                        const ctrl = TcHmi.ControlFactory.createEx(
                            'TcHmi.Controls.System.TcHmiUserControlHost',
                            `StationA_${station.nIndex}`,
                            {
                                'data-tchmi-top': 0,
                                'data-tchmi-left': 0,
                                'data-tchmi-bottom': 0,
                                'data-tchmi-right': 0,
                                'data-tchmi-grid-row-index': 0,
                                'data-tchmi-grid-column-index': station.nIndex - 1,
                                'data-tchmi-station': `%s%ADS.PLC1.GVL_Stations.A[${i}]%/s%`,
                                'data-tchmi-target-user-control': 'UserControls/StationA.usercontrol',
                            },
                            grid
                        );
                    }
                });

                stationsB.forEach((station, i) => {
                    if (station.nIndex !== 0 && station.bEnabled) {
                        const ctrl = TcHmi.ControlFactory.createEx(
                            'TcHmi.Controls.System.TcHmiUserControlHost',
                            `StationA_${station.nIndex}`,
                            {
                                'data-tchmi-top': 0,
                                'data-tchmi-left': 0,
                                'data-tchmi-bottom': 0,
                                'data-tchmi-right': 0,
                                'data-tchmi-grid-row-index': 0,
                                'data-tchmi-grid-column-index': station.nIndex - 1,
                                'data-tchmi-station': `%s%ADS.PLC1.GVL_Stations.B[${i}]%/s%`,
                                'data-tchmi-target-user-control': 'UserControls/StationB.usercontrol',
                            },
                            grid
                        );
                    }
                });
            }
            HMI.RenderStations = RenderStations;
        })(HMI = Functions.HMI || (Functions.HMI = {}));
    })(Functions = TcHmi.Functions || (TcHmi.Functions = {}));
})(TcHmi);
TcHmi.Functions.registerFunctionEx('RenderStations', 'TcHmi.Functions.HMI', TcHmi.Functions.HMI.RenderStations);
