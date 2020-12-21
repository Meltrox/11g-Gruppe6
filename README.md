## Gruppeopgave for PoP - 11g

Opgaven er lavet af:
* Mattias Matsushita 
* Johannes Rosendal 
* Johannes Andersen

## Hvordan man compiler og kører programmet ##
<img src="https://i.imgur.com/M5rsn25.gif" width="600">

## Følgende Entities findes i spillet ##

* __Player:__ Er repræsenteret som et hvidt "@" med darkmagenta (lyserød) baggrund. 
* __Wall:__ Er repræsenteret som grå områder/pixels. Spilleren kan ikke gå gennem disse, og er derfor nødt til at gå rundt om dem. 
* __Water:__ Er repræsenteret som et hvidt "W" med blå baggrund. Når spilleren går over vandet, får man 3 ekstra liv, og vandet forsvinder.
* __Fire:__ Er repræsenteret som et rødt felt #28a745, $bg-green. Hvis man går over ilden mister 1 man liv, når man har gået over ilden 5 gange, går ilden ud
* __FleshEatingPlant:__ Er repræsenteret som et hvidt "P" med grøn baggrund. Hvis prøver at gå ind i den kødeædende planter, angriber den dig, og man mister 5 liv.
* __Teleport:__ Er repræsenteret med et blåt "T" med lyserød baggrund.
* __Hints:__ Er repræsenteret med et blåt "H" med blå baggrund. Disse kan samles op, hvorved et tekststykke bliver tilføjet til spillerens inventory, der kan ses under kanvasset.
* __CodeDoor:__ Er repræsenteret med et hvidt "H" med sort baggrund. Når du prøver at gå ind i døren, så vil den be dig om at taste et kodeord/password. Dog skal du passe på
  da hvis du taster forkert, så får du stød og mister 2 hitpoints. Tryk "Enter" uden at skrive noget, for at undgå dette, hvis du stadig ikke ved hvad koden er!

## Test af de forskellige implementeringer ##

### Test af klassen "Fire" ###

Her skal spilleren miste 1HP hver gang han/hun gør oven i ilden. Ilden skal slukker når spilleren har rørt ilden 5 gange.
Hvis spilleren mister alle sine HP, dør spilleren og spillet slutter.

<img src="https://i.imgur.com/KmIR1Oi.gif" width="600">

### Test af klasserne "FleshEatingPlant, CodeDoor and Exit" ###

Spilleren mister 5HP, når man prøver at gå oven i et felt, som er optaget af en "FleshEatingPlant".

Spilleren skal ved interaktion med "CodeDoor" indtaste et password. Hvis passwordet er forkert, mister spilleren 2 HP.
Hvis passwordet er korrekt, åbnes døren.

Spilleren kan kun komme ud af grotten, hvis spilleren har 5HP eller mere tilbage.

<img src="https://i.imgur.com/OicaF8J.gif" width="600">

### Test af klasserne "Wall, Water, Teleporter og Hints" ###

Spilleren kan ikke gå igennem en mur, men der sker heller ikke noget med spilleren.

Spilleren skal få 2 HP hver gang, spilleren går oven i noget vand. Vandet skal herefter forsvinde. 
Spilleren kan maksimalt få 20HP.

Hvis spilleren går ind i en teleporter, bliver han/hun teleporteret til et nyt sted på banen.

Hvis spilleren går oveni det samme felt som et hint, så bliver hintet tilføjet til spillerens inventory, i form af en string,
og hintet forsvinder fra banen.

<img src="https://i.imgur.com/nODSBdj.gif" width="600">




