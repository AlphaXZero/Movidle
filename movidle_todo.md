# Movidle — Checklist d'améliorations


- [ ] `Login.razor.cs` — `HandleLogin` appelle `user_info.Username` sans vérifier si `user_info` est `null` → crash si identifiants incorrects
- [ ] `Home.razor.cs` — `AddToFavorites` ajoute toujours le **dernier film recherché** (`movie`), pas celui de la ligne cliquée → tous les boutons pointent sur le même film
- [ ] `year_verif` utilise `int.Parse()` sans `TryParse` → exception si la valeur n'est pas un entier valide (ex: `"2001–2003"`)
- [ ] `GetRandomMovie` peut renvoyer un doublon dans la liste `movies_pool` (`"Fight Club"` et `"Train to Busan"` apparaissent deux fois)
- [ ] `AppState` est un service Scoped/Singleton mais ne notifie pas les composants d'un changement → la navbar ne se rafraîchit pas après login sans `StateHasChanged`
- [ ] **CSS "film non trouvé"** : le message d'erreur quand OMDb ne trouve pas un film n'a pas de style dédié → ajouter classe `.alert-error` sur le `<p class="game-log">`
- [ ] **Chargement à l'inscription** : après un `Register` réussi, la page ne redirige pas / ne se recharge pas → ajouter `Navigation.NavigateTo("/")` ou auto-login après inscription
- [ ] Normaliser les titres avec chiffres romains : **convertir "III" → "3"** (et inversement) avant la comparaison OMDb/TMDB, ou tenter les deux formes en cas d'échec
- [ ] **Compteur de tentatives** : afficher `X / 6` au-dessus ou sous le tableau
- [ ] **Empêcher de soumettre un film déjà proposé** : vérifier que le titre n'est pas déjà dans `movies` avant d'ajouter
- [ ] **Message de victoire** : afficher un écran/modal stylisé quand `movie.Title == rdm_movie.Title`
- [ ] **Sauvegarder l'état des guesses en cours** : persister `movies` + `rdm_movie` en session (Blazor `ProtectedSessionStorage`) pour résister au refresh
- [ ] Stocker les films dans une db pour éviter les appels API répétés et permettre l'autocomplete (utiliser tmdb à la place pour mettre à jour la db chaque jour)
- [ ] `GetRandomMovie` pioche dans la DB locale au lieu d'appeler l'API à chaque partie
- [ ] Déconnexion (bouton logout + reset de `AppState`)
- [ ] Pouvoir supprimer un film de ses favoris
- [ ] Message d'erreur affiché si le login échoue (identifiants incorrects)
- [ ] Animation d'apparition des lignes du tableau (fade-in à chaque nouveau guess)
- [ ] Indicateur de chargement (`spinner`) pendant la recherche
