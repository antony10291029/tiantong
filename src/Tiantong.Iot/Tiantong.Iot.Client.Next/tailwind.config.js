const colors = require("tailwindcss/colors");

module.exports = {
  // purge: ["./src/**/*.{vue,js,ts,jsx,tsx}"],
  darkMode: false,
  plugins: [],
  theme: {
    screens: {

    },
    extend: {
      colors: {
        dark: {
          "900": "#202020",
          "800": "#252525",
          "300": "#b1b1b1"
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
      borderWidth: ["first", "last"],
      borderColor: ["first", "last", "active"],
      borderRadius: ["first", "last"],
      ringColor: ["active"],
      ringOpacity: ["active"],
      textColor: ["active"],
    },
  },
};
