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
    "@typescript-eslint/interface-name-prefix": ["off"],
    "@typescript-eslint/no-empty-function": ["off"],
    "arrow-parens": ["off"],
    "class-methods-use-this": "off",
    "comma-dangle": ["off", "never"],
    "constructor-super": ["off"],
    "space-before-function-paren": ["off"],
    "function-paren-newline": ["off"],
    "implicit-arrow-linebreak": ["off"],
    "import/prefer-default-export": ["off"],
    "no-console": process.env.NODE_ENV === "production" ? "warn" : "off",
    "no-debugger": process.env.NODE_ENV === "production" ? "warn" : "off",
    "no-shadow": ["off"],
    "no-useless-constructor": "off",
    "no-param-reassign": "off",
    "quote-props": ["off"],
    "quotes": ["error", "double"],
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
