const colors = require("tailwindcss/colors");

module.exports = {
  purge: ["./src/**/*.{vue,js,ts,jsx,tsx}"],
  darkMode: false,
  plugins: [],
  theme: {
    screens: {
      sm: "768px",
    },
    extend: {
      colors: {
        dark: {
          "900": "#202020"
        },
      },
    },
    colors: {
      transparent: "transparent",
      current: "currentColor",
      white: colors.white,
      black: colors.black,
      gray: colors.coolGray,
      danger: colors.red,
      warning: colors.amber,
      success: colors.emerald,
      link: colors.blue,
      dark: colors.gray,
      info: colors.sky,
      primary: colors.sky,
    }
  },
  variants: {
    extend: {
      width: ["hover", "focus"],
      backgroundColor: ["active"],
      opacity: ["disabled"],
      cursor: ["disabled"],
      borderColor: ["last"],
    },
  },
};
