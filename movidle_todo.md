# Movidle — Checklist d'améliorations


- [x] `Login.razor.cs` — `HandleLogin` appelle `user_info.Username` sans vérifier si `user_info` est `null` → crash si identifiants incorrects
- [X] Indicateur de chargement (`spinner`) pendant la recherche
- [ ] `Home.razor.cs` — `AddToFavorites` ajoute toujours le **dernier film recherché** (`movie`), pas celui de la ligne cliquée → tous les boutons pointent sur le même film
- [X] Le texte lgged in n'est pas actualisé
- [X] **Chargement à l'inscription** : après un `Register` réussi, la page ne redirige pas / ne se recharge pas → ajouter `Navigation.NavigateTo("/")` ou auto-login après inscription
- [ ] Normaliser les titres avec chiffres romains : **convertir "III" → "3"** (et inversement) avant la comparaison OMDb/TMDB, ou tenter les deux formes en cas d'échec
- [ ] **Compteur de tentatives** : afficher `X / 6` au-dessus ou sous le tableau
- [ ] **Empêcher de soumettre un film déjà proposé** : vérifier que le titre n'est pas déjà dans `movies` avant d'ajouter
- [ ] **Message de victoire** : afficher un écran/modal stylisé quand `movie.Title == rdm_movie.Title`
- [ ] **Sauvegarder l'état des guesses en cours** : persister `movies` + `rdm_movie` en session (Blazor `ProtectedSessionStorage`) pour résister au refresh
- [ ] Stocker les films dans une db pour éviter les appels API répétés et permettre l'autocomplete (utiliser tmdb à la place pour mettre à jour la db chaque jour)
- [ ] `GetRandomMovie` pioche dans la DB locale au lieu d'appeler l'API à chaque partie
- [ ] Pouvoir supprimer un film de ses favoris
- [ ] Message d'erreur affiché si le login échoue (identifiants incorrects)
- [ ] Animation d'apparition des lignes du tableau (fade-in à chaque nouveau guess)

- [ ] enter pour valider guess
- [ ] écran gris quand film non valide entré
- [ ] Normaliser noms variables
- [ ] Retenir dernier login pour pas se reco à chaque fois
- [ ] singleton/scoped
- [ ] expliquer interfacce ?
- [ ] Tests DB
- [ ] Acces db distance
- [ ] Encore + de responsive en utilisant grid de bootstrap par ex ?
- [ ] Ajouter tuto
