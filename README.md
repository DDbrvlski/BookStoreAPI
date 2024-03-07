# BookStoreAPI
"BookStoreAPI" to interfejs API służący do zarządzania funkcjami sklepu internetowego oraz systemu CMS.

Frontend zaprezentowany w przykładzie został stworzony jako część tego samego projektu inżynierskiego, integrując się bezpośrednio z tym API.
Autor projektu frontendu to [https://github.com/kmilosia].

### Solucja składa się z:
- BookStoreAPI - główna aplikacja zawierająca kontrolery, serwisy obsługujące operacje bazodanowe oraz dodatkowe funkcje typu: generowanie dokumentów pdf na podstawie szablonu w docx
- BookStoreBusinessLogic - biblioteka klas zawierająca logikę biznesową
- BookStoreData - biblioteka klas zawierająca kontekst bazy danych oraz klasy reprezentujące tabele w bazie danych
- BookStoreDTO - biblioteka klas zawierająca modele DTO służące do mapowania obiektów

## Wykorzystane technologie:

### .NET:
8.0

### Język programowania: 
- C#

### Biblioteki i frameworki:
- ASP.NET Core
- Entity Framework Core
- Identity
- QuestPDF
- Spire.Doc

## Opis aplikacji:

### **Funkcje aplikacji:**
- Zarządzanie treścią:
  - Pełna możliwość modyfikowania treści (CRUD)
  - Dodawanie i nadawanie ról dla użytkowników
  - Modyfikowanie uprawnień dla ról
- Zarządzanie zamówieniami:
  - Realizacja zamówienia dla użytkowników z kontem oraz bez
  - Generowanie faktur w formacie PDF
- Klient
  - Autoryzacja i uwierzytelnianie
  - Recenzja zakupionych produktów
  - Modyfikowanie danych osobowych
  - Historia zamówień
- Sklep
  - Wyszukiwanie produktów na podstawie różnych kryteriów
  - Używanie kodów zniżkowych w koszyku
- Komunikacja
  - Subskrybcja newslettera
  - Wysyłanie wiadomości kontaktowych do sklepu za pomocą formularza
  - Wysyłanie wiadomości email do klienta

 ## Prezentacja funkcjonalności:

### Zdjęcia

![Produkty](Images/MainPage.JPG)
<p align="center">Obraz 1. Strona główna.</p>

----
![Produkty](Images/Shop.JPG)
<p align="center">Obraz 2. Sklep.</p>

----
![Produkty](Images/Product1.JPG)
<p align="center">Obraz 3. Produkt 1.</p>

![Produkty](Images/Product2.JPG)
<p align="center">Obraz 4. Produkt 2.</p>

----
![Produkty](Images/Order.JPG)
<p align="center">Obraz 5. Zamówienie.</p>

----
![Produkty](Images/Wishlist.JPG)
<p align="center">Obraz 6. Wishlista.</p>

----
![Produkty](Images/CMSMainPage.JPG)
<p align="center">Obraz 7. Strona główna w systemie CMS.</p>

----
![Produkty](Images/CMSCreate.JPG)
<p align="center">Obraz 8. Dodawanie nowego produktu w systemie CMS.</p>

----
![Produkty](Images/CMSPermissions.JPG)
<p align="center">Obraz 9. Zarządzanie uprawnieniami w systemie CMS.</p>

----
![Produkty](Images/CMSTemplates.JPG)
<p align="center">Obraz 10. Zarządzanie szablonami dokumentów w systemie CMS.</p>

----
![Produkty](Images/InvoicePDF.JPG)
<p align="center">Obraz 11. Przykładowa faktura z szablonem.</p>

----

### Wideo

 #### Sklep internetowy
 [![Web App](https://img.youtube.com/vi/w2DbDEpGDzY/0.jpg)](https://www.youtube.com/watch?v=w2DbDEpGDzY)

----
 #### System CMS
 [![CMS](https://img.youtube.com/vi/pFkqJ6W-Y0k/0.jpg)](https://www.youtube.com/watch?v=pFkqJ6W-Y0k)
