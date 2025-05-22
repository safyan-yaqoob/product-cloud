/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./Pages/**/*.{cshtml,cs,js,ts}",
    "./Areas/**/*.{cshtml,cs,js,ts}",
    "./Views/**/*.{cshtml,cs,js,ts}",
    "./wwwroot/**/*.{js,ts}"
  ],
  theme: {
    extend: {},
  },
  plugins: [
    require('@tailwindcss/forms')
  ],
  // This ensures Tailwind classes take precedence
  important: true,
}