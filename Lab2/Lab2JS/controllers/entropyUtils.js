const fs = require('fs');

// Метод для подсчета вероятности появления символа в тексте
function calculateCharacterProbability(text, character) {
  const cleanedText = text.toLowerCase().replace(/[\s.,\/#!$%\^&\*;:{}=\-_`~()]/g, '');
  const totalCharacters = cleanedText.length;
  const characterCount = cleanedText.split(character).length - 1;
  return characterCount / totalCharacters;
}

// Функция для расчета энтропии
function calculateEntropy(probabilities) {
  let entropy = 0;
  for (const character of Object.keys(probabilities)) {
    const probability = probabilities[character];
    if (probability != 0.00000) {
      entropy += probability * Math.log2(probability);
    }
    else {
      entropy += 0; // Добавляем 0 для нулевой вероятности
    }
  }
  entropy = -entropy.toFixed(5);
  return entropy;
}

function calculateInformationQuantity(entropy, mesLength) {
  return entropy * mesLength;
}

function calculateBinaryEntropy(probability0, probability1) {
  const entropy = -probability0 * Math.log2(probability0) - probability1 * Math.log2(probability1);
  return entropy;
}

function convertToBinary(text) {
  let binaryText = '';
  const cleanedText = text.toLowerCase().replace(/[\s.,\/#!$%\^&\*;:{}=\-_`~()]/g, '');
  for (let i = 0; i < cleanedText.length; i++) {
    const charCode = cleanedText.charCodeAt(i);
    const binaryCode = charCode.toString(2).padStart(8, '0');
    // const binaryCode = charCode < 128 ? charCode.toString(2).padStart(8, '0') : charCode.toString(2).padStart(16, '0'); // Дополняем до 8 бит для латинских символов и до 16 бит для кириллических символов
    binaryText += binaryCode.padStart(8, '0'); // Дополняем до 8 бит
  }
  return binaryText;
}

function calculateBinEntropyWithError(p) {
  const q = 1 - p;
  var entropy = 0;
  if (p !== 1) {
    entropy = (1 - ((-p * Math.log2(p)) - (q * Math.log2(q))));
  } else {
    entropy = (1 - ((-p * Math.log2(p))));
  }
  if (isNaN(entropy)) {
    return 0;
  }

  return entropy;
}
function calculateTextEntropyWithError(probabilities, p) {
  const q = 1 - p;
  let entropy = 0;
  const conditionalEntropy = 1 - ((-p * Math.log2(p)) - (q * Math.log2(q)));
  if (p >= 0.5) {
    return 0;
  }
  for (const character of Object.keys(probabilities)) {
    const probability = probabilities[character];
    if (probability != 0.00000) {
      entropy -= (probability * Math.log2(probability)) * conditionalEntropy;
    }
    else {
      entropy -= 0;
    }
  }
  return entropy;
}

function getEntropies() {
  const portText = fs.readFileSync('public/portug.txt', 'utf8');
  const serbText = fs.readFileSync('public/serbian.txt', 'utf8');

  const portugAlphabet = [
    'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k',
    'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v',
    'w', 'x', 'y', 'z'
  ];

  const serbAlphabet = [
    '\u0430', '\u0431', '\u0432', '\u0433', '\u0434', '\u0452',
    '\u0435', '\u0436', '\u0437', '\u0438', '\u0458', '\u043A',
    '\u043B', '\u0459', '\u043C', '\u043D', '\u045A', '\u043E',
    '\u043F', '\u0440', '\u0441', '\u0442', '\u045B', '\u0443',
    '\u0444', '\u0445', '\u0446', '\u0447', '\u045F', '\u0448'
  ];

  const portCharacterProbabilities = {};
  for (const character of portugAlphabet) {
    const portProbability = calculateCharacterProbability(portText, character);
    portCharacterProbabilities[character] = portProbability.toFixed(5);
  }
  const portEntropy = calculateEntropy(portCharacterProbabilities);

  const serbCharacterProbabilities = {};
  for (const character of serbAlphabet) {
    const serbProbability = calculateCharacterProbability(serbText, character);
    serbCharacterProbabilities[character] = serbProbability.toFixed(5);
  }
  const serbEntropy = calculateEntropy(serbCharacterProbabilities);

  return { portCharacterProbabilities, portEntropy, serbCharacterProbabilities, serbEntropy };
}

function getBinaryEntropies() {
  const portText = fs.readFileSync('public/portug.txt', 'utf8');
  const serbText = fs.readFileSync('public/serbian.txt', 'utf8');

  const portBinaryText = convertToBinary(portText);
  const serbBinaryText = convertToBinary(serbText);

  const portProbability0 = calculateCharacterProbability(portBinaryText, '0');
  const portProbability1 = calculateCharacterProbability(portBinaryText, '1');
  const serbProbability0 = calculateCharacterProbability(serbBinaryText, '0');
  const serbProbability1 = calculateCharacterProbability(serbBinaryText, '1');

  const portBinaryEntropy = calculateBinaryEntropy(portProbability0, portProbability1);
  const serbBinaryEntropy = calculateBinaryEntropy(serbProbability0, serbProbability1);

  return { portBinaryText, serbBinaryText, portProbability0, portProbability1, portBinaryEntropy, serbProbability0, serbProbability1, serbBinaryEntropy };
}

module.exports = { getEntropies, getBinaryEntropies, convertToBinary, calculateInformationQuantity, calculateTextEntropyWithError, calculateBinEntropyWithError };