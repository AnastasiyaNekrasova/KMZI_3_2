const fs = require('fs');
const mjAPI = require('mathjax-node');
const { getBinaryEntropies } = require('./entropyUtils');

function calculateProbabilitySecond(req, res) {
  const portText = fs.readFileSync('public/portug.txt', 'utf8');
  const serbText = fs.readFileSync('public/serbian.txt', 'utf8');

  const formula = 'H(A_2) = -p(0)*log_2(p(0))-p(1)*log_2(p(1)) = ';
  mjAPI.typeset({
    math: formula,
    format: 'TeX',
    svg: true
  }, function (data) {
    if (!data.errors) {
      const svgFormula = data.svg;

      const { portBinaryText, serbBinaryText, portProbability0, portProbability1, portBinaryEntropy, serbProbability0, serbProbability1, serbBinaryEntropy } = getBinaryEntropies();

      res.render('secondTask', {
        formula: svgFormula,
        portText,
        portLength: portText.length,
        portBinaryText,
        portBinaryLength: portBinaryText.length,
        portProbability0,
        portProbability1,
        portBinaryEntropy: portBinaryEntropy.toFixed(5),
        serbText,
        serbLength: serbText.length,
        serbBinaryText,
        serbBinaryLength: serbBinaryText.length,
        serbProbability0,
        serbProbability1,
        serbBinaryEntropy: serbBinaryEntropy.toFixed(5),
      });
    } else {
      console.log('Ошибка при обработке формулы:', data.errors);
    }
  });

}

module.exports = { calculateProbabilitySecond };