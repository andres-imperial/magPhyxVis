import numpy

import talib
# followed roughly https://blog.quantinsti.com/install-ta-lib-python/
# installed ta-lib by downloading the cp38 win_amd64.whl file from
# https://www.lfd.uci.edu/~gohlke/pythonlibs/#ta-lib 
# and running pip install <path to .whl file>

print("Trying out talib")

numDataPoints = 100
headings = ['n', 'event_type', 't', 'close-price']

stockData = {
    'open': numpy.random.random(numDataPoints),
    'high': numpy.random.random(numDataPoints),
    'low': numpy.random.random(numDataPoints),
    'close': numpy.random.random(numDataPoints),
    'volume': numpy.random.random(numDataPoints)
}

def myFirstTradingAlgo(inputs):
    results = []
    for openPrice in inputs["open"]:
        if openPrice > 0.5:
            results.append("buy")
        else:
            results.append("sell")
    return results


def trader2(inputs, params, outputFile):
    results = []
    mom = talib.MOM(inputs["close"], timeperiod=10)
    std_devs = talib.STDDEV(inputs['close'], timeperiod=10, nbdev=1)

    # iterate through all time periods, deciding to buy, sell, or stay
    i = 1
    for (momentum, std_dev) in zip(mom, std_devs):
        if momentum > params["mom_trigger_buy"] and std_dev < params['std_dev']:
            results.append("buy")
            addEvent(i, "buy", i, inputs["close"][i], outputFile)
        elif momentum < params["mom_trigger_sell"] and std_dev < params['std_dev']:
            results.append("sell")
            addEvent(i, "sell", i, inputs["close"][i], outputFile)
        else:
            results.append("stay")
            addEvent(i, "stay", i, inputs["close"][i], outputFile)
    return results

# coefficients - an array of 23 coefficients corresponding to the 23 included momentum indicators
def momentumTrader(marketData, coefficients, file):
    results = []
    indicators = []
    period = 10
    fp = 12
    sp = 26
    matyp = 0

    # add all momentum indicators to the indicators array
    indicators.append(talib.ADX(marketData["high"], marketData["low"], marketData["close"], timeperiod=period))
    indicators.append(talib.ADXR(marketData["high"], marketData["low"], marketData["close"], timeperiod=period))
    indicators.append(talib.APO(marketData["close"], fastperiod=fp, slowperiod=sp, matype=matyp))
    #indicators.append(talib.AROON(marketData["high"], marketData["low"], timeperiod=period))
    indicators.append(talib.AROONOSC(marketData["high"], marketData["low"], timeperiod=period))
    indicators.append(talib.BOP(marketData["open"], marketData["high"], marketData["low"], marketData["close"]))
    indicators.append(talib.CCI(marketData["high"], marketData["low"], marketData["close"], timeperiod=period))
    indicators.append(talib.CMO(marketData["close"], timeperiod=period))
    indicators.append(talib.DX(marketData["high"], marketData["low"], marketData["close"], timeperiod=period))
    # skipped a few
    indicators.append(talib.MFI(marketData["high"], marketData["low"], marketData["close"], marketData["volume"], timeperiod=period))
    indicators.append(talib.MINUS_DI(marketData["high"], marketData["low"], marketData["close"], timeperiod=period))
    indicators.append(talib.MINUS_DM(marketData["high"], marketData["low"], timeperiod=period))
    indicators.append(talib.MOM(marketData["close"], timeperiod=period))
    indicators.append(talib.PLUS_DI(marketData["high"], marketData["low"], marketData["close"], timeperiod=period))
    indicators.append(talib.PLUS_DM(marketData["high"], marketData["low"], timeperiod=period))
    indicators.append(talib.PPO(marketData["close"], fastperiod=fp, slowperiod=sp, matype=matyp))
    indicators.append(talib.ROC(marketData["close"], timeperiod=period))
    indicators.append(talib.ROCP(marketData["close"], timeperiod=period))
    indicators.append(talib.ROCR(marketData["close"], timeperiod=period))
    indicators.append(talib.ROCR100(marketData["close"], timeperiod=period))
    indicators.append(talib.RSI(marketData["close"], timeperiod=period))
    indicators.append(talib.TRIX(marketData["close"], timeperiod=period))
    indicators.append(talib.ULTOSC(marketData["high"], marketData["low"], marketData["close"], timeperiod1=7, timeperiod2=14, timeperiod3=28))
    indicators.append(talib.WILLR(marketData["high"], marketData["low"], marketData["close"], timeperiod=period))

    # iterate through all time periods, deciding to buy, sell, or stay
    #i = 1
    for i in range(len(indicators[0])):
        score = calculateMomentumScore(indicators, coefficients, i)
        if score > 0.5:
            results.append("buy")
            addEvent(i, "buy", i, marketData["close"][i], file)
        elif score < -0.5:
            results.append("sell")
            addEvent(i, "sell", i, marketData["close"][i], file)
        else:
            results.append("stay")
            addEvent(i, "stay", i, marketData["close"][i], file)
    return results

def calculateMomentumScore(indicators, coefficients, timeindex):
    score = 0
    for (indicator, coefficient) in zip(indicators, coefficients):
        if not numpy.isnan(indicator[timeindex]):
            score += indicator[timeindex] * coefficient
    return score

def printHeadings(headings, file):
    for h in headings:
        file.write(h)
        file.write(',')
    file.write('\n')

# helper function to print events
def addEvent(n, event_type, t, price, file):
    file.write(f"{n},{event_type},{t},{price}\n")


#for i in range(8):
#    for j in range(8):
#        outputFile = (f"trading_events{i*j}{j}.csv")
#        trader2Params = {
#            'mom_trigger_buy': 0.1 * i,
#            'mom_trigger_sell': -0.1 * i,
#            'std_dev': 0.1 * j 
#        }
#        print("*** Running another trading simulation with params %s***" % str(trader2Params))
#        buySells = trader2(stockData, trader2Params, outputFile)
#        print(buySells)

numIndicators = 23
for i in range(numIndicators):
    #coeffs = [0] * 23
    filename = './../../data/momentumTradingData/events/events' + f'{i:02}' + '.csv'
    f = open(filename, 'w')
    printHeadings(headings, f)

    coeffs = numpy.zeros(numIndicators)
    coeffs[i] = 1
    results = momentumTrader(stockData, coeffs, f)
    print("######################################")
    print(results)
    f.close()

print("Experiment success")
