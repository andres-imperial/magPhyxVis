let data_sets = [
    {
        name: 'Trading Data',
        sim_count: 144,
        events_folder: 'momentumTradingData/events',
        param_folder: null,
        parse: parseStockMarketData,
    },
    {
        name: 'MagPhyx Grid',
        sim_count: 99,
        events_folder: 'data4/events',
        param_folder: 'data4/commands',
        parse: parseMagPhyxData,
    },
    {
        name: 'MagPhyx Random (7)',
        sim_count: 99,
        events_folder: 'data7/events',
        param_folder: 'data7/commands',
        parse: parseMagPhyxData,
    }
];

/**
 * Data parsers should structure the data as follows:
 * 
 * data = {
 *   simulations = [],
 *   eventTypes = []
 * }
 * 
 * simulations = [
 *   { // first simulation
 *     params:
 *     meta:
 *     events: []
 *   },
 *   { // second simulation
 *     params:
 *     meta:
 *     events: []
 *   }
 * ]
 * 
 */
function parseStockMarketData(eventData) {
    let simulations = [];
    let eventTypes = ['buy', 'sell', 'stay'];

    for (let i = 0; i < eventData.length; i++) {
        let oneSim = eventData[i];

        simulations.push(
            {
                params: null,
                meta: {},
                events: []
            }
        )
        for (let j = 0; j < oneSim.length; j++) {
            simulations[i].events.push(oneSim[j]);
            simulations[i].events[j].t = +oneSim[j][" t"];
            simulations[i].events[j].on = false;
            simulations[i].events[j].selected = false;
        }
    }

    return {
        simulations: simulations,
        eventTypes: eventTypes,
        getColor: getStockMarketColor
    };
}

function getStockMarketColor(event) {
    switch (event.category) {
        case 'Volume Indicators':
            return 'grey';
        case 'Volatility Indicators':
            return 'orange';
        case 'Statistic Functions':
            return 'yellow';
        case 'Price Transform':
            return 'brown';
        case 'Pattern Recognition':
            return 'blue';
        case 'Overlap Studies':
            return 'purple';
        case 'Momentum Indicators':
            return 'green';
        case 'Math Transform':
            return 'cyan';
        case 'Math Operators':
            return 'red';
        case 'Cycle Indicators':
            return 'pink';
        default:
            return 'black';
    }
}

const baseUrl = 'http://edwardsjohnmartin.github.io/MagPhyx/?initparams=';
/**
 * given a row number, create a url so the simulation can be replayed in MagPhyx
 * sample url: http://edwardsjohnmartin.github.io/MagPhyx/?initparams=1,0,0,0.721905,-0.0589265,0.0455992
 */ 
function getMagPhyxUrl(paramData) {
    let p = paramData; // shorter alias
    let result = `${baseUrl}${p.r},${p.angleFromOrigin},${p.momentAngle},${p.theta},${p.beta},${p.angularMomentum}`;
    return result;
  }

/**
 * Parse data generated by the MagPhyx simulation
 * @param eventData a 2D array of simulations/data events
 * @param paramData an array of the parameters used to generate the simulations
 * @returns an data object
 */
function parseMagPhyxData(eventData, paramData) {
    let simulations = [];
    let eventTypes = ['collision', 'beta = 0', 'pr = 0', 'pphi = 0', 'ptheta = 0'];

    for (let i = 0; i < eventData.length; i++) {
        let oneSim = eventData[i];
        simulations.push(
            {
                params: paramData[i],
                meta: {},
                events: []
            }
        )
        // give each simulation a URL based on the params
        simulations[i].meta['url'] = getMagPhyxUrl(paramData[i]);

        for (let j = 0; j < oneSim.length; j++) {
            let oneEvent = oneSim[j];
            simulations[i].events.push({
                n: +oneEvent.n,
                event_type: oneEvent[' event_type'],
                beta: +oneEvent[' beta'],
                phi: +oneEvent[' phi'],
                pphi: +oneEvent[' pphi'],
                ptheta: +oneEvent[' ptheta'],
                theta: +oneEvent[' theta'],
                r: +oneEvent[' r'],
                t: +oneEvent[' t'],
                on: false,
                selected: false
            });
            // simulations[i].events[j].t = +oneEvent[' t'];
        }
    }

    return {
        simulations: simulations,
        eventTypes: eventTypes,
        getColor: getMagPhyxColor
    };
}

function getMagPhyxColor(event) {
    switch (event['event_type']) {
        case 'init':
            return 'grey';
        case 'collision':
            return 'red';
        case 'beta = 0':
            return 'purple';
        case 'theta = 0':
            return 'blue';
        case 'pr = 0':
            return 'orange';
        case 'phi = 0':
            return 'green';
        case 'pphi = 0':
            return 'brown';
        default:
            return 'black';
    }
}
