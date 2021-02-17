module.exports = {
  root: true,
  env: {
    node: true,
  },
  extends: [
    "plugin:vue/vue3-essential",
    "@vue/airbnb",
    "@vue/typescript/recommended",
  ],
  parserOptions: {
    ecmaVersion: 2020,
  },
  rules: {
    "@typescript-eslint/camelcase": ["off"],
    "@typescript-eslint/interface-name-prefix": ["off"],
    "@typescript-eslint/no-empty-function": ["off"],
    "@typescript-eslint/no-explicit-any": ["off"],
    "arrow-parens": ["off"],
    "class-methods-use-this": "off",
    "comma-dangle": ["off", "never"],
    "constructor-super": ["off"],
    "function-paren-newline": ["off"],
    "implicit-arrow-linebreak": ["off"],
    "import/prefer-default-export": ["off"],
    "no-console": process.env.NODE_ENV === "production" ? "warn" : "off",
    "no-debugger": process.env.NODE_ENV === "production" ? "warn" : "off",
    "no-multi-assign": ["off"],
    "no-param-reassign": "off",
    "no-plusplus": ["off"],
    "no-return-assign": ["off"],
    "no-shadow": ["off"],
    "no-useless-constructor": "off",
    "object-curly-newline": ["off"],
    "quote-props": ["off"],
    "quotes": ["error", "double"],
    "space-before-function-paren": ["off"],
    "vue/no-mutating-props": ["off"],
  },
  overrides: [
    {
      files: [
        "**/__tests__/*.{j,t}s?(x)",
        "**/tests/unit/**/*.spec.{j,t}s?(x)",
      ],
      env: {
        jest: true,
      },
    },
  ],
};