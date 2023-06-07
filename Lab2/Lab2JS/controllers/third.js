const mjAPI = require('mathjax-node');
const { convertToBinary, getEntropies, getBinaryEntropies, calculateInformationQuantity } = require('./entropyUtils');

function calculateProbabilityThird(req, res) {
    const portMes = 'Nekrasova Anastasia Pavlovna'
    const serbMes = 'Некрасова Анастасија Павловна'

    const portText = portMes.toLowerCase().replace(/[\s.,\/#!$%\^&\*;:{}=\-_`~()]/g, '');
    const portBinaryText = convertToBinary(portText)
    const serbText = serbMes.toLowerCase().replace(/[\s.,\/#!$%\^&\*;:{}=\-_`~()]/g, '');
    const serbBinaryText = convertToBinary(serbText)

    const { portEntropy, serbEntropy } = getEntropies();
    const { portBinaryEntropy, serbBinaryEntropy } = getBinaryEntropies();
    
    const portOriginalInfQuan = calculateInformationQuantity(portEntropy, portText.length)
    const portBinInfQuan = calculateInformationQuantity(portBinaryEntropy, portBinaryText.length)
    const serbOriginalInfQuan = calculateInformationQuantity(serbEntropy, serbText.length)
    const serbBinInfQuan = calculateInformationQuantity(serbBinaryEntropy, serbBinaryText.length)

    const formula = 'I(X_k) = H(A)*k';
    mjAPI.typeset({
        math: formula,
        format: 'TeX',
        svg: true
    }, function (data) {
        if (!data.errors) {
            const svgFormula = data.svg;
            // Рендеринг шаблона с формулой
            res.render('thirdTask', {
                formula: svgFormula,
                portMes: portMes,
                serbMes: serbMes,
                portMesLength: portText.length,
                serbMesLength: serbText.length,
                portBinaryText,
                portBinaryLength: portBinaryText.length,
                serbBinaryText,
                serbBinaryLength: serbBinaryText.length,
                portEntropy,
                serbEntropy,
                portBinaryEntropy,
                serbBinaryEntropy,
                portOriginalInfQuan: portOriginalInfQuan.toFixed(5),
                portBinInfQuan: portBinInfQuan.toFixed(5),
                serbOriginalInfQuan: serbOriginalInfQuan.toFixed(5),
                serbBinInfQuan: serbBinInfQuan.toFixed(5)
            });
        } else {
            console.log('Ошибка при обработке формулы:', data.errors);
        }
    });

}

module.exports = { calculateProbabilityThird };