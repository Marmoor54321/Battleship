# Battleship  

Gra Battleship stworzona w celu nauki praktycznego wykorzystania wzorców projektowych.
![image](https://github.com/user-attachments/assets/cbb1f39e-4b83-4f0d-a89b-44acd57d2ac3)
# Spis treści
- [O projekcie](#o-projekcie)
- [Funkcjonalności](#funkcjonalności)
- [Instalacja](#instalacja)
- [Użyte wzorce](#wzorce)
## O projekcie
Gra **Battleship** została stworzona w ramach przedmiotu akademickiego,
którego celem było praktyczne zastosowanie wzorców projektowych w programowaniu.
Projekt umożliwił zgłębienie zasad projektowania wzorców, takich jak Observer, Composite czy Memento, oraz ich implementację w rzeczywistej aplikacji. Ze względu na lokalny charakter gry oraz ułatwienie testowania, statki obu graczy są widoczne na planszy.
## Funkcjonalności  
- Rozgrywka między dwoma graczami lub z komputerem
- Obrót statku za pomocą prawego przycisku myszy
- Historia rozgrywek
- Statystyki
- System osiągnięć
- "Skórki" statków  
## Instalacja
### Wymagania
- System operacyjny: Windows 10 lub nowszy 
- [Visual Studio 2022](https://visualstudio.microsoft.com/)
- [.NET 6.0 SDK](https://dotnet.microsoft.com/download) lub nowsze
- [MonoGame 3.8.1](http://www.monogame.net/downloads/) (zainstaluj szablony MonoGame dla Visual Studio)
### Sklonuj repozytorium
```bash
git clone https://github.com/Marmoor54321/Battleship.git
cd Battleship
```
## Wzorce  
- Composite: Modeluje relację "część-całość" w strukturze statków.
Ship (całość) składa się z wielu ShipPart (części), co pozwala traktować statek jako jedną jednostkę, a jednocześnie umożliwia zarządzanie jego komponentami indywidualnie.  
- Memento: GameHistory Przechowuje historię gier jako pojedyncza instancja, zapewniając, że dane historyczne są scentralizowane i dostępne z dowolnej części aplikacji.
- Template Method: Wzorzec definiuje ogólną strukturę rozgrywki w klasie bazowej GameMode, umożliwiając specyficzne implementacje w klasach potomnych, takich jak StandardMode i SimulationMode.
Wspólne metody, jak initializeGame, playTurn, czy endGame, mogą być dostosowywane przez poszczególne tryby gry.  
- Observer: Achievements nasłuchuje zdarzeń w grze, takich jak ukończenie zadania lub zwycięstwo, aby odblokować odpowiednie osiągnięcia.
Ułatwia dynamiczne reagowanie na zmiany w stanie gry.
