<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="registerDocumentPL.aspx.cs" Inherits="Eurona.User.Advisor.RegisterDocumentPL" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        body{font-size:12px;font-family:helvetica, Tahoma, Verdana;}
        .lable{font-size:10px;color:#494949;}
        .value span{ font-size:10px;color:#000;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table border="0" width="800px" style="margin:auto;">
            <tr>
                <td colspan="3" align="center">
                    <strong>REJESTRACJA</strong><br />
                    NIEZALEŻNEGO ZAREJESTROWANEGO DORADCY-KONSULTANTA* / KLIENTA* 
                    SPÓŁKI EURONA s.r.o.
                    UMOWA O ODBIORZE I DYSTRYBUCJI PRODUKTÓW<br /><br />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                <strong>1.</strong>Dane wypełni sponsor wspólnie z nowym Niezależnym Zarejestrowanym Doradcą – Konsultantem */ Klientem*.<br />
                <strong>2.</strong>Do podania o rejestrację należy załączyć pierwsze zamówienie zainteresowanego, a Doradca – Konsultant załączy jeszcze kopię zaświadczenia o wpisie do ewidencji działalności gospodarczej.<br />
                <strong>3.</strong>Razem z rejestracją można podać pierwsze zamówienie.<br />
                <strong>4.</strong>Niezależny Zarejestrowany Doradca – Konsultant / Klient poda numer rachunku bankowego.<br />
                <strong>5.</strong><br />
                </td>
            </tr>
            <tr>
                <td rowspan="2">
                    <strong>6. Odbiorca - Niezależny Doradca - Konsultant* / Klient*</strong></td>
                <td class="lable">
                    numer rejestracyjny</td>
                <td class="lable">
                    data rejestracji</td>
            </tr>
            <tr>
                <td class="value" style="border-style: solid; border-width: 1px">
                    <asp:Label ID="lblRegistracniCislo" runat="server" Text="&nbsp;"></asp:Label>
                </td>
                <td class="value" style="border-style: solid; border-width: 1px">
                    <asp:Label ID="lblDatumRegistrace" runat="server" Text="&nbsp;"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="lable"> 
                    nazwisko</td>
                <td class="lable">
                    imię (imiona) zgodnie z aktem urodzenia</td>
                <td class="lable">
                    tytuł</td>
            </tr>
            <tr>
                <td class="value" style="border-style: solid; border-width: 1px">
                    <asp:Label ID="lblPrijmeni" runat="server" Text="&nbsp;"></asp:Label>
                </td>
                <td class="value" style="border-style: solid; border-width: 1px">
                    <asp:Label ID="lblJmeno" runat="server" Text="&nbsp;"></asp:Label>
                </td>
                <td class="value" style="border-style: solid; border-width: 1px">
                    <asp:Label ID="lblTitul" runat="server" Text="&nbsp;"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="lable">
                    nazwa handlowa-firma</td>
                <td class="lable">
                    data urodzenia</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="value" style="border-style: solid; border-width: 1px">
                    <asp:Label ID="lblNazev" runat="server" Text="&nbsp;"></asp:Label>
                </td>
                <td class="value" style="border-style: solid; border-width: 1px">
                    <asp:Label ID="lblDatumNarozeni" runat="server" Text="&nbsp;"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="lable">
                    REGON</td>
                <td class="lable">
                    NIP</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="value" style="border-style: solid; border-width: 1px">
                    <asp:Label ID="lblICO" runat="server" Text="&nbsp;"></asp:Label>
                </td>
                <td class="value" style="border-style: solid; border-width: 1px">
                    <asp:Label ID="lblDIC" runat="server" Text="&nbsp;"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="lable">
                    adres siedziby</td>
                <td>
                    &nbsp;</td>
                <td class="lable">
                    kod pocztowy</td>
            </tr>
            <tr>
                <td class="value" colspan="2" style="border-style: solid; border-width: 1px">
                    <asp:Label ID="lblAdresaSidla" runat="server" Text="&nbsp;"></asp:Label>
                </td>
                <td class="value" style="border-style: solid; border-width: 1px">
                    <asp:Label ID="lblPSCSidlo" runat="server" Text="&nbsp;"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="lable">
                    adres dla doręczeń</td>
                <td>
                    &nbsp;</td>
                <td class="lable">
                    kod pocztowy</td>
            </tr>
            <tr>
                <td class="value" colspan="2" style="border-style: solid; border-width: 1px">
                    <asp:Label ID="lblAdresaDorucovaci" runat="server" Text="&nbsp;"></asp:Label>
                </td>
                <td class="value" style="border-style: solid; border-width: 1px">
                    <asp:Label ID="lblPSCDorucovaci" runat="server" Text="&nbsp;"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="lable">
                    tel.domowy (też kierunkowy)</td>
                <td class="lable">
                    tel. komórkowy</td>
                <td class="lable">
                    tel.do pracy</td>
            </tr>
            <tr>
                <td class="value" style="border-style: solid; border-width: 1px">
                    <asp:Label ID="lblTelefonDomu" runat="server" Text="&nbsp;"></asp:Label>
                </td>
                <td class="value" style="border-style: solid; border-width: 1px">
                    <asp:Label ID="lblMobilniTelefon" runat="server" Text="&nbsp;"></asp:Label>
                </td>
                <td class="value" style="border-style: solid; border-width: 1px">
                    <asp:Label ID="lblTelefonDoZamestnani" runat="server" Text="&nbsp;"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="lable">adres poczty elektronicznej (e-mail)</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="value" colspan="3" style="border-style: solid; border-width: 1px">
                    <asp:Label ID="lblEmail" runat="server" Text="&nbsp;"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="lable">
                    bank</td>
                <td class="lable">
                    numer rachunku</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="value" style="border-style: solid; border-width: 1px">
                    <asp:Label ID="lblBanka" runat="server" Text="&nbsp;"></asp:Label>
                </td>
                <td class="value" style="border-style: solid; border-width: 1px">
                    <asp:Label ID="lblCisloUctu" runat="server" Text="&nbsp;"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <strong>7. Sponsor</strong></td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="lable">
                    imię i nazwisko Twojego sponsora</td>
                <td class="lable">
                    numer rejestracyjny</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="value" style="border-style: solid; border-width: 1px">
                    <asp:Label ID="lblJmenoSponzora" runat="server" Text="&nbsp;"></asp:Label>
                </td>
                <td class="value" style="border-style: solid; border-width: 1px">
                    <asp:Label ID="lblRegistracniCisloSponzora" runat="server" Text="&nbsp;"></asp:Label>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="3">
                    * niepotrzebne skreślić<br /><br />
                    <strong>WARUNKI UMOWY:</strong>
                    <br /><br />
                    <div style="text-align:justify;">
                    <strong>8.</strong>Niezależny Zarejestrowany Doradca – Konsultant / Klient przyjmuje do wiadomości, że podpisując rejestrację i umowę nie zostaje pracownikiem spółki  EURONA s.r.o. (zwanej dalej EURONA) ani swojego sponsora. Niezależny Zarejestrowany Doradca – Konsultant / Klient nie jest upoważniony występować w imieniu spółki EURONA ani czynić jakiekolwiek kroki prawne w jej imieniu.<br />
                    <strong>9.</strong>Niezależny Zarejestrowany Doradca – Konsultant / Klient wyraża zgodę z opublikowaniem jego imienia i nazwiska w materiałach spółki, przede wszystkim w czasopiśmie EURONA – NEWS.<br />
                    <strong>10.</strong>Niezależny Zarejestrowany Doradca – Konsultant / Klient  zobowiązuje się, że wszelkie informacje dotyczące spółki EURONA, ew. jej produktów, które przekazuje innym są zgodne z oficjalną polityką i z materiałami szkoleniowymi spółki EURONA, a zwłaszcza z publikacją „Droga sukcesu”.<br />
                    <strong>11.</strong>Produkty EURONA będą dystrybuowane wyłącznie drogą sprzedaży bezpośredniej.<br />
                    <strong>12.</strong>Niezależny Zarejestrowany Doradca – Konsultant / Klient zobowiązuje się zachować tajemnicę i nie udzielać informacji, które mogłyby działać na korzyść innych podmiotów prawnych, bądź w inny sposób szkodzić interesom spółki EURONA. Zobowiązuje się również, że nie wykorzysta sieci dystrybucyjnej spółki EURONA do sprzedaży innych produktów ani do innych celów.<br />
                    <strong>13.</strong>Niezależny Zarejestrowany Doradca – Konsultant / Klient oświadcza, że on ani jego żona / mąż nie był zarejestrowany w ciągu ubiegłych 6-ciu miesięcy przed niniejszą rejestracją w spółce EURONA.<br />
                    <strong>14.</strong>Zgadzam się, aby spółka EURONA wykorzystała moje dane osobiste zawarte w niniejszej umowie do zarządzania siecią dystrybucyjną i siecią detaliczną produktów i usług EURONA, do przekazywania tych danych zainteresowanym pracą Niezależnego Zarejestrowanego Doradcy –Konsultanta / Klienta EURONA, czy też zainteresowanym kupnem produktów i usług EURONA. Zgadzam się również z tym, aby moje dane osobiste były wykorzystane w akcjach marketingowych EURONA. Zgoda wchodzi w życie w dniu podpisania aż do ewentualnego pisemnego odwołania przesłanego pod adres głównej kancelarii spółki EURONA.<br />
                    <strong>15.</strong>Niezależny Zarejestrowany Doradca – Konsultant / Klient powinien odbierać i dystrybuować produkty na swoje nazwisko, swoim kosztem i na własną odpowiedzialność.<br />
                    <strong>16.</strong>Jeżeli odbiorę towar za innego Niezależnego Zarejestrowanego Doradcę – Konsultanta / Klienta, a ten nie zapłaci za towar w terminie płatności, zobowiązuję się wyrównać całkowicie powstałą należność.<br />
                    <strong>17.KARY UMOWNE</strong><br />
                    <strong>18.</strong>System spółki EURONA będzie mieć treść ewidencji kredytu i bonusu, ich pośrednictwem będzie możliwe każdemu (odbiorcy) klientu obniżyć lub podwyższyć prawo do prowizji w odpowiednej dłużnej kwocie albo odpowiedniego prawa do bonusu.<br />
                    <strong>19.</strong>Jeżeli Niezależny Zarejestrowany Doradca – Konsultant / Klient ma wobec spółki EURONA dług ponad 60 dni po terminie płatności, EURONA zastrzega sobie prawo opublikować i przekazać informacje o Niezależnym Zarejestrowanym Doradcy – Konsultancie / Kliencie stronie trzeciej z celem wyegzekwowania należności.<br />
                    <strong>20.</strong>Odbiorca zobowiązuje się odebrać zamówiony towar i wyrównać jego cenę w wysokości i w terminie płatności podanym na fakturze wystawionej przez dostawcę, którą otrzyma przy osobistym odbiorze albo kiedy towar dostarczy umowna firma wysyłkowa.<br />
                    <strong>21.</strong>Obie strony mają prawo jednostronnie rozwiązać rejestrację i umowę za dwumiesięcznym okresem wypowiedzenia. W wypadku złamania warunków umowy może spółka EURONA natychmiest jednostronnie anulować rejestrację i umowę.<br />
                    <strong>22.</strong>EURONA zastrzega sobie prawo korygować plan marketingowy i zasady podane w publikacji „Droga sukcesu“ jak również treść niniejszej umowy. Jeżeli Niezależny Zarejestrowany Doradca – Konsultant / Klient nie zgadza się ze zmianami w publikacji „Droga sukcesu”, bądź w innych dokumentach spółki EURONA, może poinformować o tym na piśmie spółkę EURONA. Jeżeli Niezależny Zarejestrowany Doradca – Konsultant / Klient daje zamówienie spółce EURONA po oznajmieniu przez nią tych zmian (łącznie z publikacjami), uważa się, że Doradca – Konsultant zmiany przyjął do wiadomości.<br />
                    <strong>23.</strong>Niezależny Zarejestrowany Doradca – Konsultant / Klient upoważnia niniejszym spółkę EURONA, aby zgodnie z planem marketingowym opisanym w publikacji „Droga sukcesu”, dokonywała obliczeń, alokacji i wypłacała prowizje i bonusy, jak również obliczała marżę detaliczną-narzut procentowy w zależności od wielkości sprzedaży.<br />
                    <strong>24.</strong>Rejestracja i umowa są sporządzone w dwu jednobrzmiących egzemplarzach, po jednym dla każdej strony umowy.<br />
                    <strong>25.</strong>Rejestracja wchodzi w życie i nabiera mocy prawnej w dniu jej podpisania przez obie strony umowy, tj. w dniu……………..<br />
                    </div>
                    <br />
                    <asp:CheckBox runat="server" Text="poczta" /> &nbsp;&nbsp;&nbsp;Centrum dystrybucyjne:…………………………………………………………
                    <br /><br /><br /><br />
                    <table width="100%">
                    <tr>
                        <td>…………………………………….</td>
                        <td>…………………………………….</td>
                    </tr>
                    <tr>
                        <td>podpis zainteresowanego</td>
                        <td>podpis sponsora</td>
                    </tr>
                    </table>
                    <br /><br />
                    <strong>26.Dostawca</strong>
                    <br />
                    Eurona s.r.o., Lhota za Červeným Kostelcem 261, Červený Kostelec 549 41,Česká republika, numer identyfikacyjny (REGON): 25996304
                    <br />
                    Wpisana do Rejestru Handlowego prowadzonego przez Sąd Okręgowy Hradec Králové, dział C, numer 19017.
                    <br /><br />
                    <strong>Dyrektor EURONA s.r.o.</strong><br />
                    <strong>Lukáš Černý</strong>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
