<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="registerDocument.aspx.cs" Inherits="Eurona.User.Advisor.RegisterDocument" %>

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
                    <strong>PŘIHLÁŠKA - REGISTRACE</strong><br />
                    NEZÁVISLÉHO REGISTROVANÉHO PORADCE* / ZÁKAZNÍKA*<br /> 
                    SPOLEČNOSTI EURONA s.r.o.<br />
                    A SMLOUVA O ODBĚRU A DISTRIBUCI VÝROBKŮ<br />
                    UZAVŘENÁ VE SMYSLU § 51 OBČ. ZÁKONÍKU<br /><br />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                <strong>1.</strong>Údaje vyplní sponzor společně s novým nezávislým poradcem*/zákazníkem*.<br />
                <strong>2.</strong>K přihlášce - registraci je nutno, v případě poradce, přiložit kopii živnostenského listu, případně kopii registrace plátce DPH.<br />
                <strong>3.</strong>Společně s přihláškou je nutné přiložit první objednávku.<br />
                <strong>4.</strong>Nezávislý registrovaný poradce uvede číslo svého bankovního účtu.<br />
                <strong>5.</strong>V kontaktech prosím vyplňte váš mobilní telefon pro snazší a rychlejší informatiku a komunikaci mezi společností EURONA a vámi.<br />
                </td>
            </tr>
            <tr>
                <td rowspan="2">
                    <strong>6. Odběratel - nezávislý poradce */zákazník *</strong></td>
                <td class="lable">
                    registrační čilso</td>
                <td class="lable">
                    datum registrace</td>
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
                    příjmení</td>
                <td class="lable">
                    jméno v rodném liste</td>
                <td class="lable">
                    titul</td>
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
                    obchodní jméno</td>
                <td class="lable">
                    datum narození</td>
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
                    IČO</td>
                <td class="lable">
                    DIČ</td>
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
                    adresa sídla(ulice čislo, město)</td>
                <td>
                    &nbsp;</td>
                <td class="lable">
                    psč</td>
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
                    doručovací adresa</td>
                <td>
                    &nbsp;</td>
                <td class="lable">
                    psč</td>
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
                    telefon domů</td>
                <td class="lable">
                    mobilní telefon</td>
                <td class="lable">
                    telefon do zaměstnání</td>
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
                <td class="lable">
                    e-mailová adresa</td>
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
                    banka</td>
                <td class="lable">
                    číslo úctu</td>
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
                    <strong>7. Sponzor</strong></td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="lable">
                    jméno a příjmení Vašeho sponzora</td>
                <td class="lable">
                    registrační číslo</td>
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
                    Pozn: * nehodící se škrtněte<br /><br />
                    <strong>SMLUVNÍ PODMÍNKY:</strong>
                    <br /><br />
                    <div style="text-align:justify;">
                    <strong>8.</strong>Nezávislý registrovaný poradce/zákazník bere na vědomí, že podpisem přihlášky - registrace a smlouvy se nestává zaměstnancem společnosti EURONA s.r.o. (dále jen EURONA) ani svého sponzora. Nezávislý registrovaný poradce/zákazník není oprávněn jednat a činit jakékoli právní úkony jménem společnosti EURONA.<br />
                    <strong>9.</strong>Nezávislý registrovaný poradce/zákazník souhlasí, aby jeho jméno bylo publikováno v interních materiálech společnosti, zejména v časopise EURONA – NEWS.<br />
                    <strong>10.</strong>Nezávislý registrovaný poradce/zákazník se zavazuje k tomu, že veškeré jím šířené informace o společnosti  EURONA, resp. jejích výrobcích budou plně v souladu s oficiální politikou a literaturou společnosti EURONA, zejména publikací Obchodní manuál.<br />
                    <strong>11.</strong>Výrobky EURONA budou konečným zákazníkům distribuovány výhradně přímým prodejem.<br />
                    <strong>12.</strong>Nezávislý registrovaný poradce/zákazník se zavazuje dodržovat obchodní tajemství a neposkytovat informace, jež by mohly zvýhodňovat jiné právní subjekty, či jinak poškozovat zájmy společnosti EURONA. Dále se zavazuje nezneužívat distribuční sítě EURONA k prodeji jiných výrobků, ani k jiným účelům.<br />
                    <strong>13.</strong>Nezávislý registrovaný poradce/zákazník prohlašuje, že on ani jeho(její) manžel(ka) nebyl(a) zaregistrován(a) v období 6-ti měsíců před touto registrací ve společnosti EURONA.<br />
                    <strong>14.</strong>V souladu se zák.č.101/2000 Sb.souhlasím se zpracováním mých osobních údajů na této smlouvě společností EURONA za účelem vytvoření a provozování distribuční a spotřebitelské sítě pro výrobky a služby EURONA, za účelem poskytování těchto údajů zájemcům o registraci jako Nezávislého registrovaného poradce/zákazníka EURONA, či zájemcům o nákup výrobků a služeb EURONA. Dále souhlasím, že mé osobní údaje mohou být využity pro marketingové akce EURONA. Souhlas je platný dnem podpisu, až do jeho případného písemného odvolání na adresu hlavní kanceláře společnosti EURONA.<br />
                    <strong>15.</strong>Nezávislý registrovaný poradce/zákazník je povinen odběr a distribuci výrobků vykonávat pod svým jménem, na své náklady a vlastní odpovědnost, a to v souladu s platnými právními předpisy ČR.<br />
                    <strong>16.</strong>Pokud převezmu zboží za jiného Nezávislého registrovaného poradce/zákazníka a ten neuhradí zboží ve lhůtě splatnosti, zavazuji se tímto vzniklou pohledávku uhradit v plné výši.<br />
                    <strong>17.</strong>V případě, že platba nebude identifikována v důsledku neuvedení variabilního a specifického symbolu a v souvislosti s tím vzniknou náklady na vymáhání pohledávky, zavazuji se tyto náklady soudní a právního zastoupení uhradit.<br />
                    <strong>18.SMLUVNÍ SANKCE</strong><br />
                    <strong>18.1.</strong>Odběratel se zavazuje při nesplnění povinnosti úhrady za převzetí zboží ve stanoveném termínu splatnosti zaplatit dodavateli poplatek či úrok z prodlení v souladu s ust.§ 1 a §2 NAŘÍZENÍ vlády ČR č.142/1994 Sb.<br />
                    <strong>18.2.</strong>Kromě výše uvedených sankcí se odběratel zavazuje zaplatit dodavateli za nesplnění svých povinností vyplývajících z této smlouvy pokutu ve výši:<br />
                    a)při překročení termínu splatnosti do 30 dnů od termínu splatnosti v částce 150,-Kč.<br />
                    b)při překročení termínu splatnosti o více než 30 dnů v částce 450,-Kč.<br />
                    <strong>18.3.</strong>Peněžitá pokuta bude připočtena k základní dlužné částce a bude spolu s ní základem pro výpočet poplatku dle odst. 18.1.<br />
                    <strong>18.4.</strong>Systém společnosti EURONA bude obsahovat evidenci dluhů a bonusů, jejímž prostřednictvím bude možné každému odběrateli krátit či navyšovat nárok na provizi v adekvátní výši odpovídající dlužné částce nebo příslušného nároku na bonus. <br />
                    <strong>19.</strong>V případě, že má Nezávislý registrovaný poradce/zákazník vůči společnosti EURONA dluh více než 60 dnů po datu splatnosti, EURONA si vyhrazuje právo zveřejnit a předat informace o tomto Nezávislém registrovaném poradci/zákazníkovi třetí straně za účelem vymáhání dluhu.<br />
                    <strong>20.</strong>Odběratel se zavazuje převzít objednané zboží a uhradit jeho cenu ve výši a v termínu splatnosti uvedeném na faktuře vystavené dodavatelem, kterou obdrží buď při osobním odběru, nebo při dodávce zboží smluvní zasílatelskou společností.<br />
                    <strong>21.</strong>Obě strany jsou oprávněny jednostranně ukončit registraci a smlouvu s dvouměsíční výpovědní lhůtou. V případě porušení smluvních podmínek může být ze strany EURONA registrace a smlouva jednostranně okamžitě zrušena.<br />
                    <strong>22.</strong>EURONA si vyhrazuje možnost upravovat marketingový plán a pravidla uvedená v publikaci Obchodní manuál a Obchodní podmínky i text této smlouvy. Pokud Nezávislý registrovaný poradce/zákazník nesouhlasí se změnami v publikaci Obchodní manuál a Obchodní podmínky, či v dalších dokumentech EURONA, může společnost EURONA informovat písemnou formou. Pokud Nezávislý registrovaný poradce/zákazník podává objednávky u EURONA i poté, co EURONA tyto změny řádné oznámila (včetně interních publikací), jsou tyto změny považovány za poradcem akceptované.<br />
                    <strong>23.</strong>Nezávislý registrovaný poradce/zákazník tímto pověřuje společnost EURONA, aby v souladu s marketingovým plánem EURONA uvedeným v publikaci Obchodní manuál a Obchodní podmínky, prováděl výpočet, alokaci a výplatu provizí a bonusů a výpočet obchodní přirážky z obratu.<br />
                    <strong>24.</strong>Přihláška - Registrace a smlouva je vyhotovena ve třech stejnopisech, z nichž každá ze smluvních stran obdrží jedno vyhotovení. Jednu kopii (3. list) obdrží sponzor.<br />
                    <strong>25.</strong>Platnost a právní účinnost registrace a smlouvy nastávají dnem podpisu oběma smluvními stranami, tj. dnem……………..<br />
                    </div>
                    <br />
                    <asp:CheckBox runat="server" Text="pošta" /> &nbsp;&nbsp;&nbsp;Distribuční centrum:…………………………………………………………
                    <br /><br /><br /><br />
                    <table width="100%">
                    <tr>
                        <td>…………………………………….</td>
                        <td>…………………………………….</td>
                    </tr>
                    <tr>
                        <td>podpis žadatele</td>
                        <td>podpis sponzora</td>
                    </tr>
                    </table>
                    <br /><br />
                    <strong>26.Dodavatel</strong>
                    <br />
                    Eurona s.r.o., Lhota za Červeným Kostelcem 261, Červený Kostelec 549 41,Česká republika, IČO: 25996304
                    <br />
                    Zapsáno v Obchodním rejstříku vedeném u Krajského obchodního soudu v Hradci Králové v oddílu C, vložka číslo 19017.
                    <br /><br />
                    <strong>Ředitel EURONA s.r.o.</strong><br />
                    <strong>Lukáš Černý</strong>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
