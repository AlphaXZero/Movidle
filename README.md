# Movidle - Small game where you have to guess a movie name.
The app is live at [movidle-production.up.railway.app](https://movidle-production.up.railway.app/)
## How to Play
- Type a movie title in the search bar and press **Guess**
- Each guess reveals how close you are — every cell is color-coded:
  - 🟢 **Green** — exact match
  - 🟠 **Orange** — partial match (genres and countries)
  - 🔴 **Red** — wrong
  - ↑ / ↓ — for year and metascore, indicates if the answer is higher or lower
- Use the clues to narrow down the mystery movie in as few guesses as possible!
## Installation
create API key on [omdbapi.com](https://www.omdbapi.com/apikey.aspx),
and add the key in the `appsettings.json`:

## Tests
`dotnet test` to run the tests located in `Tests/GameTests.cs`

Covers:
- User service (register, login, favorites)
- Game state (guess history, reset)
- Verification logic (title, year, genre matching)

## Informations
- CSS and Tests file were been made with the help of claudeai.
- See `misc/todo` for potential upcoming improvements.
- I made smalls test (for db connection) in another project.
