# Movidle — Checklist d'améliorations


- [x] `Login.razor.cs` — `HandleLogin` appelle `user_info.Username` sans vérifier si `user_info` est `null` → crash si identifiants incorrects
- [X] Indicateur de chargement (`spinner`) pendant la recherche
- [X] `Home.razor.cs` — `AddToFavorites` ajoute toujours le **dernier film recherché** (`movie`), pas celui de la ligne cliquée → tous les boutons pointent sur le même film
- [X] Le texte lgged in n'est pas actualisé
- [X] **Chargement à l'inscription** : après un `Register` réussi, la page ne redirige pas / ne se recharge pas → ajouter `Navigation.NavigateTo("/")` ou auto-login après inscription
- [X] écran gris quand film non valide entré
- [X] **Sauvegarder l'état des guesses en cours** : persister `movies` + `rdm_movie` en session (Blazor `ProtectedSessionStorage`) pour résister au refresh
- [X] Message d'erreur affiché si le login échoue (identifiants incorrects)
- [x] **Compteur de tentatives** : afficher `X / 6` au-dessus ou sous le tableau
- [ ] Stocker les films dans une db pour éviter les appels API répétés et permettre l'autocomplete (utiliser tmdb à la place pour mettre à jour la db chaque jour)
- [ ] Normaliser les titres avec chiffres romains : **convertir "III" → "3"** (et inversement) avant la comparaison OMDb/TMDB, ou tenter les deux formes en cas d'échec
- [ ] Animation d'apparition des lignes du tableau (fade-in à chaque nouveau guess)
- [ ] Retenir dernier login dans localstorage pour pas se reco à chaque fois dans
- [ ] Encore + de responsive en utilisant grid de bootstrap par ex ?
- [ ] Ajouter tuto pour expliquer le jeu
- [ ] Enlever AppState.Username car inutile ? évite recherche dans db ) voir

URGENT
- [X] **Message de victoire** : afficher un écran/modal stylisé quand `movie.Title == rdm_movie.Title`
- [X] Problème DB à distance
- [x] **Empêcher de soumettre un film déjà proposé** : vérifier que le titre n'est pas déjà dans `movies` avant d'ajouter
- [X] Pouvoir supprimer un film de ses favoris
- [x] enter pour valider guess
- [ ] Normaliser noms variables
- [ ] Improve readme
- [ ] Tests DB
- [ ] singleton/scoped
- [ ] Utilisation interface ?
