# Arena
Egyszerű szöveges körökre osztott 1v1 harcjáték szimulátor (.NET, C#, console app)

### Specifikáció
Az arénában N db hősök küzdenek, akik lehetnek íjászok, lovasok és kardosok. Minden hős
rendelkezik egy azonosítóval és életerővel, valamint a lenti szabály szerint tudnak támadni és
védekezni.

Íjász támad
<ul>
    <li>lovast: 40% eséllyel a lovas meghal, 60%-ban kivédi</li>
    <li>kardost: kardos meghal</li>
    <li>íjászt: védekező meghal</li>
</ul>

Kardos támad
<ul>
    <li>lovast: nem történik semmi</li>
    <li>kardost: védekező meghal</li>
    <li>íjászt: íjász meghal</li>
</ul>

Lovas támad
<ul>
    <li>lovast: védekező meghal</li>
    <li>kardost: lovas meghal</li>
    <li>íjászt: íjász meghal</li>
</ul>

A csata körökre van lebontva, minden körbe véletlenszerűen kiválasztásra kerül egy támadó és egy
védekező. A kimaradt hősök pihennek és növekszik az életerejük 10-el, viszont nem mehet a
maximum fölé.</br>
A harcban résztvevő hősök életereje a felére csökken, ha ez kisebb mint a kezdeti életerő negyede
akkor meghalnak. Kezdeti életerők íjász: 100 lovas: 150 kardos: 120.</br>

A csata elindítása előtt lekell generálni N darab véletlenszerű hőst, amit paraméterként fog
megkapni. Csata addig tart még maximum 1 hős marad elétben.</br>

Minden kör végén logolni kell ki támadott meg kit és hogyan változott az életerejük.</br>

Készíts egy olyan consol applikációt ami a fenti szabályt figyelembe véve hősöket csatáztat
egymással. 
