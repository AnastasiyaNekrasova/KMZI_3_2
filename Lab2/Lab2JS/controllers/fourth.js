const fs = require('fs');
const mjAPI = require('mathjax-node');
const { calculateTextEntropyWithError, calculateBinEntropyWithError,  convertToBinary, getEntropies } = require('./entropyUtils');

function calculateProbabilityFourth(req, res) {
    const portMes = 'Nekrasova Anastasia Pavlovna'
    const serbMes = 'Некрасова Анастасија Павловна'

    const portText = portMes.toLowerCase().replace(/[\s.,\/#!$%\^&\*;:{}=\-_`~()]/g, '');
    const portBinaryText = convertToBinary(portText)
    const serbText = serbMes.toLowerCase().replace(/[\s.,\/#!$%\^&\*;:{}=\-_`~()]/g, '');
    const serbBinaryText = convertToBinary(serbText)

    const { portCharacterProbabilities, serbCharacterProbabilities } = getEntropies();

    const formula = 'H_e(A) = 1 - H(Y|X)';
    const secondFormula = 'H(Y|X) = -p * \\log_2(p) - q * \\log_2(q)';
    mjAPI.typeset({
        math: formula,
        format: 'TeX',
        svg: true
    }, function (data) {
        if (!data.errors) {
            const svgFormula = data.svg;

            mjAPI.typeset({
                math: secondFormula,
                format: 'TeX',
                svg: true
            }, function (secondData) {
                if (!secondData.errors) {
                    const svgSecondFormula = secondData.svg;

                    const portInfQuanWithErr01 = calculateTextEntropyWithError(portCharacterProbabilities, 0.1) * portText.length
                    const portBinInfQuanWithErr01 = calculateBinEntropyWithError(0.1) * portBinaryText.length
                    const portInfQuanWithErr05 = calculateTextEntropyWithError(portCharacterProbabilities, 0.5) * portText.length
                    const portBinInfQuanWithErr05 = calculateBinEntropyWithError(0.5) * portBinaryText.length
                    const portInfQuanWithErr1 = calculateTextEntropyWithError(portCharacterProbabilities, 1) * portText.length
                    const portBinInfQuanWithErr1 = calculateBinEntropyWithError(1) * portBinaryText.length

                    const serbInfQuanWithErr01 = calculateTextEntropyWithError(serbCharacterProbabilities, 0.1) * serbText.length
                    const serbBinInfQuanWithErr01 = calculateBinEntropyWithError(0.1) * serbBinaryText.length
                    const serbInfQuanWithErr05 = calculateTextEntropyWithError(serbCharacterProbabilities, 0.5) * serbText.length
                    const serbBinInfQuanWithErr05 = calculateBinEntropyWithError(0.5) * serbBinaryText.length
                    const serbInfQuanWithErr1 = calculateTextEntropyWithError(serbCharacterProbabilities, 1) * serbText.length
                    const serbBinInfQuanWithErr1 = calculateBinEntropyWithError(1) * serbBinaryText.length

                    res.render('fourthTask', { 
                        formula: svgFormula, 
                        secondFormula: svgSecondFormula,
                        portInfQuanWithErr01,
                        portBinInfQuanWithErr01,
                        portInfQuanWithErr05,
                        portBinInfQuanWithErr05,
                        portInfQuanWithErr1,
                        portBinInfQuanWithErr1,
                        serbInfQuanWithErr01,
                        serbBinInfQuanWithErr01,
                        serbInfQuanWithErr05,
                        serbBinInfQuanWithErr05,
                        serbInfQuanWithErr1,
                        serbBinInfQuanWithErr1
                     });
                } else {
                    console.log('Ошибка при обработке второй формулы:', secondData.errors);
                }
            });
        } else {
            console.log('Ошибка при обработке первой формулы:', data.errors);
        }
    });
}

module.exports = { calculateProbabilityFourth };
