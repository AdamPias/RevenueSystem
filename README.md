# RevenueSystem

RevenueSystem to backendowa aplikacja przeznaczona do zarządzania bazą klientów, generowania kontraktów na sprzedaż oprogramowania, obsługi płatności ratalnych oraz dynamicznego obliczania przychodów (rzeczywistych i prognozowanych). System wykorzystuje architekturę Clean Architecture i integruje się z zewnętrznym API NBP w celu obsługi wielu walut.

## 🛠 Technologie i Architektura

Aplikacja oparta jest na nowoczesnym stosie technologicznym .NET i architekturze warstwowej (Clean Architecture), która zapewnia niezależność logiki biznesowej od infrastruktury[cite: 1, 2, 3, 4, 8].

*   **Język i Framework:** C#, .NET (ASP.NET Core Web API)[cite: 4, 5, 6, 7].
*   **Baza danych:** Entity Framework Core (wzorzec Code First, relacyjna baza danych)[cite: 8].
*   **Autoryzacja:** JSON Web Tokens (JWT) z podziałem na role oraz systemem tokenów odświeżających (Refresh Tokens)[cite: 4, 7, 10, 14].
*   **Architektura (Warstwy):**
    *   **Api:** Kontrolery REST (np. `ClientsController`, `ContractsController`)[cite: 4, 5].
    *   **Application:** Logika biznesowa, autoryzacja, zarządzanie kontraktami, obliczanie przychodów (serwisy i interfejsy repozytoriów)[cite: 9, 10, 12, 13].
    *   **Domain:** Modele encji (np. `Client`, `Contract`, `Payment`)[cite: 8].
    *   **Infrastructure:** Dostęp do danych (EF Core), migracje oraz integracje z usługami zewnętrznymi (API NBP)[cite: 8, 11].

## 🚀 Kluczowe Funkcjonalności

### 1. Zarządzanie Klientami
*   **Dwa typy klientów:** Obsługa klientów indywidualnych (z wymaganym, unikalnym numerem PESEL) i korporacyjnych (z unikalnym numerem KRS)[cite: 4, 8, 9].
*   **Bezpieczne usuwanie:** Klienci firmowi nie mogą zostać usunięci. Dla klientów indywidualnych zaimplementowano mechanizm "Soft Delete", który ukrywa dane, ale zachowuje je w bazie w celach audytowych (zapobieganie utracie powiązań z fakturami)[cite: 4, 9, 15].
*   **Ograniczenia dostępu:** Edycja i usuwanie danych klientów są zastrzeżone wyłącznie dla administratorów (`[Authorize(Roles = "Admin")]`)[cite: 4].

### 2. Kontrakty i Subskrypcje Oprogramowania
*   **Zasady kontraktów:** Oferty są ważne w określonym przedziale czasowym (od 3 do 30 dni)[cite: 13]. Klient może mieć tylko jedną aktywną umowę na dany produkt[cite: 13].
*   **Dynamiczny system zniżek:** Zniżki (% procentowe) nakładają się na siebie. System pobiera najwyższą aktualną zniżkę z bazy, a stali klienci (powracający) automatycznie otrzymują dodatkowe 5% rabatu[cite: 8, 13].
*   **Wsparcie:** Możliwość dokupienia dodatkowych lat wsparcia technicznego w stałej cenie (1000 PLN za rok)[cite: 13].

### 3. Moduł Płatności (Raty i Zwroty)
*   **Częściowe płatności:** Klienci mogą opłacać kontrakt w ratach w czasie trwania okresu ważności oferty (3-30 dni)[cite: 5, 13, 16].
*   **Automatyczne zawieranie umowy:** Kontrakt (`IsSigned`) staje się automatycznie podpisany, gdy suma wpłat pokryje 100% kwoty umowy[cite: 13].
*   **Polityka zwrotów:** Jeśli termin ważności oferty minie, a kontrakt nie zostanie opłacony w całości, wpłacone dotychczas środki zostają automatycznie oznaczone jako zwrócone (`IsRefunded = true`), a klient musi wygenerować nową ofertę[cite: 13].

### 4. Kalkulacja Przychodów (Rzeczywiste vs Prognozowane)
*   System umożliwia wyliczanie przychodów dla całej firmy lub per konkretny produkt oprogramowania[cite: 6, 12, 18].
*   **Przychód rzeczywisty (Actual Revenue):** Kwota pochodząca z w pełni podpisanych i opłaconych kontraktów[cite: 6, 12].
*   **Przychód prognozowany (Projected Revenue):** Suma umów podpisanych oraz aktywnych (oczekujących na spłatę całościową w terminie)[cite: 6, 12].

### 5. Integracja z NBP (Przeliczanie Walut)
*   Przychody domyślnie obliczane są w polskich złotych (PLN)[cite: 6, 11, 12].
*   System pozwala na podanie dowolnego 3-literowego kodu waluty (np. USD, EUR) i na żywo odpytuje **API Narodowego Banku Polskiego (NBP)** o aktualny średni kurs wymiany z tabeli A lub B, zwracając wynik w wybranej walucie[cite: 11, 12, 17].

## 🔒 Uwierzytelnianie i Bezpieczeństwo
Aplikacja wykorzystuje system uwierzytelniania oparty na tokenach JWT[cite: 7, 10, 14]:
*   Logowanie z użyciem bcrypt (szyfrowanie haseł)[cite: 7, 10].
*   Krótkożyjące tokeny dostępu (15 minut)[cite: 10].
*   Długożyjące tokeny odświeżające zapisywane w bazie danych, pozwalające na przedłużenie sesji bez konieczności ponownego podawania hasła[cite: 7, 10].
