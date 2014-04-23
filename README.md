IClientRepository
=================

Twórcy
---
 - Zięba
 - Jagodzińska

Opis
---
Serwis, który pozwala tworzyć klientów i uzyskiwać informację o stworzonych klientach.
Powinien mieć kilka metod:
 - createClient()
  - przekazujemy jakąś strukturę danych zawierającą informację o kliencie (imie, nazwisko, data urodzenia, adres, itd.)
  - zwraca identyfikator klienta
 - getClientInformation()
  - przekazemy id klienta i zwraca strukture taka jak powyżej
 - getClientInformation()
  - zamiast id przyjmuje imie i nazwisko
