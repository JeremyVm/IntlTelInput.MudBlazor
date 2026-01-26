/**
 * Build script to copy intl-tel-input assets from node_modules to wwwroot
 */
const fs = require('fs');
const path = require('path');

const sourceBase = path.join(__dirname, 'node_modules', 'intl-tel-input', 'build');
const destBase = path.join(__dirname, 'wwwroot');

// Files to copy: [source (relative to build/), destination (relative to wwwroot/)]
const filesToCopy = [
  ['js/intlTelInput.min.js', 'js/intlTelInput.js'],
  ['js/utils.js', 'js/utils.js'],
  ['css/intlTelInput.min.css', 'css/intlTelInput.min.css'],
  ['css/intlTelInput.css', 'css/intlTelInput.css'],
];

// Image files to copy from img/ directory
const imageFiles = [
  'flags.png',
  'flags@2x.png',
  'flags.webp',
  'flags@2x.webp',
  'globe.webp',
  'globe@2x.webp',
];

function ensureDir(dir) {
  if (!fs.existsSync(dir)) {
    fs.mkdirSync(dir, { recursive: true });
    console.log(`Created directory: ${dir}`);
  }
}

function copyFile(src, dest) {
  if (!fs.existsSync(src)) {
    console.error(`Source file not found: ${src}`);
    process.exit(1);
  }
  ensureDir(path.dirname(dest));
  fs.copyFileSync(src, dest);
  console.log(`Copied: ${path.relative(__dirname, src)} -> ${path.relative(__dirname, dest)}`);
}

console.log('Copying intl-tel-input assets to wwwroot...\n');

// Copy main files
for (const [srcRel, destRel] of filesToCopy) {
  const src = path.join(sourceBase, srcRel);
  const dest = path.join(destBase, destRel);
  copyFile(src, dest);
}

// Copy image files
for (const imgFile of imageFiles) {
  const src = path.join(sourceBase, 'img', imgFile);
  const dest = path.join(destBase, 'img', imgFile);
  copyFile(src, dest);
}

console.log('\nAssets copied successfully!');
