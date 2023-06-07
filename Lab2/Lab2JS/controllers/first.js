const fs = require('fs');
const { getEntropies } = require('./entropyUtils');
const mjAPI = require('mathjax-node');

function calculateProbabilityFirst(req, res) {
  const formula = 'H_S(A) = -\\sum_{i=1}^{N} P(a_i) * log_2P(a_i) = ';
  mjAPI.typeset({
    math: formula,
    format: 'TeX',
    svg: true
  }, function (data) {
    if (!data.errors) {
      const svgFormula = data.svg;

      const { portCharacterProbabilities, portEntropy, serbCharacterProbabilities, serbEntropy } = getEntropies();

      res.render('firstTask', {
        portText: fs.readFileSync('public/portug.txt', 'utf8'),
        portCharacterProbabilities,
        portEntropy,
        serbText: fs.readFileSync('public/serbian.txt', 'utf8'),
        serbCharacterProbabilities,
        serbEntropy,
        formula: svgFormula
      });
    } else {
      console.log('Ошибка при обработке формулы:', data.errors);
    }
  });
}

module.exports = { calculateProbabilityFirst };