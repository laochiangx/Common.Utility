using System.Text;

namespace DotNet.Utilities
{
    public class PinYin
    {
        public string GetFirstLetter(string hz)
        {
            string ls_second_eng = "CJWGNSPGCGNESYPBTYYZDXYKYGTDJNNJQMBSGZSCYJSYYQPGKBZGYCYWJKGKLJSWKPJQHYTWDDZLSGMRYPYWWCCKZNKYDGTTNGJEYKKZYTCJNMCYLQLYPYQFQRPZSLWBTGKJFYXJWZLTBNCXJJJJZXDTTSQZYCDXXHGCKBPHFFSSWYBGMXLPBYLLLHLXSPZMYJHSOJNGHDZQYKLGJHSGQZHXQGKEZZWYSCSCJXYEYXADZPMDSSMZJZQJYZCDJZWQJBDZBXGZNZCPWHKXHQKMWFBPBYDTJZZKQHYLYGXFPTYJYYZPSZLFCHMQSHGMXXSXJJSDCSBBQBEFSJYHWWGZKPYLQBGLDLCCTNMAYDDKSSNGYCSGXLYZAYBNPTSDKDYLHGYMYLCXPYCJNDQJWXQXFYYFJLEJBZRXCCQWQQSBNKYMGPLBMJRQCFLNYMYQMSQTRBCJTHZTQFRXQ" +
         "HXMJJCJLXQGJMSHZKBSWYEMYLTXFSYDSGLYCJQXSJNQBSCTYHBFTDCYZDJWYGHQFRXWCKQKXEBPTLPXJZSRMEBWHJLBJSLYYSMDXLCLQKXLHXJRZJMFQHXHWYWSBHTRXXGLHQHFNMNYKLDYXZPWLGGTMTCFPAJJZYLJTYANJGBJPLQGDZYQYAXBKYSECJSZNSLYZHZXLZCGHPXZHZNYTDSBCJKDLZAYFMYDLEBBGQYZKXGLDNDNYSKJSHDLYXBCGHXYPKDJMMZNGMMCLGWZSZXZJFZNMLZZTHCSYDBDLLSCDDNLKJYKJSYCJLKOHQASDKNHCSGANHDAASHTCPLCPQYBSDMPJLPCJOQLCDHJJYSPRCHNWJNLHLYYQYYWZPTCZGWWMZFFJQQQQYXACLBHKDJXDGMMYDJXZLLSYGXGKJRYWZWYCLZMSSJZLDBYDCFCXYHLXCHYZJQSFQAGMNYXPFRKSSB" +
         "JLYXYSYGLNSCMHCWWMNZJJLXXHCHSYDSTTXRYCYXBYHCSMXJSZNPWGPXXTAYBGAJCXLYSDCCWZOCWKCCSBNHCPDYZNFCYYTYCKXKYBSQKKYTQQXFCWCHCYKELZQBSQYJQCCLMTHSYWHMKTLKJLYCXWHEQQHTQHZPQSQSCFYMMDMGBWHWLGSSLYSDLMLXPTHMJHWLJZYHZJXHTXJLHXRSWLWZJCBXMHZQXSDZPMGFCSGLSXYMJSHXPJXWMYQKSMYPLRTHBXFTPMHYXLCHLHLZYLXGSSSSTCLSLDCLRPBHZHXYYFHBBGDMYCNQQWLQHJJZYWJZYEJJDHPBLQXTQKWHLCHQXAGTLXLJXMSLXHTZKZJECXJCJNMFBYCSFYWYBJZGNYSDZSQYRSLJPCLPWXSDWEJBJCBCNAYTWGMPAPCLYQPCLZXSBNMSGGFNZJJBZSFZYNDXHPLQKZCZWALSBCCJXJYZGWKYP" +
         "SGXFZFCDKHJGXDLQFSGDSLQWZKXTMHSBGZMJZRGLYJBPMLMSXLZJQQHZYJCZYDJWBMJKLDDPMJEGXYHYLXHLQYQHKYCWCJMYYXNATJHYCCXZPCQLBZWWYTWBQCMLPMYRJCCCXFPZNZZLJPLXXYZTZLGDLDCKLYRZZGQTGJHHHJLJAXFGFJZSLCFDQZLCLGJDJCSNCLLJPJQDCCLCJXMYZFTSXGCGSBRZXJQQCTZHGYQTJQQLZXJYLYLBCYAMCSTYLPDJBYREGKLZYZHLYSZQLZNWCZCLLWJQJJJKDGJZOLBBZPPGLGHTGZXYGHZMYCNQSYCYHBHGXKAMTXYXNBSKYZZGJZLQJDFCJXDYGJQJJPMGWGJJJPKQSBGBMMCJSSCLPQPDXCDYYKYFCJDDYYGYWRHJRTGZNYQLDKLJSZZGZQZJGDYKSHPZMTLCPWNJAFYZDJCNMWESCYGLBTZCGMSSLLYXQSXSBSJS" +
         "BBSGGHFJLWPMZJNLYYWDQSHZXTYYWHMCYHYWDBXBTLMSYYYFSXJCSDXXLHJHFSSXZQHFZMZCZTQCXZXRTTDJHNNYZQQMNQDMMGYYDXMJGDHCDYZBFFALLZTDLTFXMXQZDNGWQDBDCZJDXBZGSQQDDJCMBKZFFXMKDMDSYYSZCMLJDSYNSPRSKMKMPCKLGDBQTFZSWTFGGLYPLLJZHGJJGYPZLTCSMCNBTJBQFKTHBYZGKPBBYMTTSSXTBNPDKLEYCJNYCDYKZDDHQHSDZSCTARLLTKZLGECLLKJLQJAQNBDKKGHPJTZQKSECSHALQFMMGJNLYJBBTMLYZXDCJPLDLPCQDHZYCBZSCZBZMSLJFLKRZJSNFRGJHXPDHYJYBZGDLQCSEZGXLBLGYXTWMABCHECMWYJYZLLJJYHLGBDJLSLYGKDZPZXJYYZLWCXSZFGWYYDLYHCLJSCMBJHBLYZLYCBLYDPDQYSXQZB" +
         "YTDKYXJYYCNRJMDJGKLCLJBCTBJDDBBLBLCZQRPXJCGLZCSHLTOLJNMDDDLNGKAQHQHJGYKHEZNMSHRPHQQJCHGMFPRXHJGDYCHGHLYRZQLCYQJNZSQTKQJYMSZSWLCFQQQXYFGGYPTQWLMCRNFKKFSYYLQBMQAMMMYXCTPSHCPTXXZZSMPHPSHMCLMLDQFYQXSZYJDJJZZHQPDSZGLSTJBCKBXYQZJSGPSXQZQZRQTBDKYXZKHHGFLBCSMDLDGDZDBLZYYCXNNCSYBZBFGLZZXSWMSCCMQNJQSBDQSJTXXMBLTXZCLZSHZCXRQJGJYLXZFJPHYMZQQYDFQJJLZZNZJCDGZYGCTXMZYSCTLKPHTXHTLBJXJLXSCDQXCBBTJFQZFSLTJBTKQBXXJJLJCHCZDBZJDCZJDCPRNPQCJPFCZLCLZXZDMXMPHJSGZGSZZQJYLWTJPFSYASMCJBTZKYCWMYTCSJJLJCQLWZM" +
         "ALBXYFBPNLSFHTGJWEJJXXGLLJSTGSHJQLZFKCGNNDSZFDEQFHBSAQTGLLBXMMYGSZLDYDQMJJRGBJTKGDHGKBLQKBDMBYLXWCXYTTYBKMRTJZXQJBHLMHMJJZMQASLDCYXYQDLQCAFYWYXQHZ";

            string ls_second_ch = "Ў°ЎҐЎ£Ў§Ў•Ў¶ЎІЎ®Ў©Ў™ЎЂЎђЎ≠ЎЃЎѓЎ∞Ў±Ў≤Ў≥ЎіЎµЎґЎЈЎЄЎєЎЇЎїЎЉЎљ" +
         "ЎЊЎњЎјЎЅЎ¬Ў√ЎƒЎ≈Ў∆Ў«Ў»Ў…Ў ЎЋЎћЎЌЎќЎѕЎ–Ў—Ў“Ў”Ў‘Ў’Ў÷Ў„ЎЎЎўЎЏЎџЎ№ЎЁЎёЎяЎаЎбЎвЎгЎдЎеЎжЎзЎиЎйЎкЎлЎмЎнЎоЎпЎрЎсЎтЎуЎфЎхЎцЎчЎшЎщЎъЎыЎьЎэЎюў°ўҐў£ў§ў•ў¶ўІў®ў©ў™ўЂўђў≠ўЃўѓў∞ў±ў≤ў≥ўіўµўґўЈўЄўєўЇўїўЉўљўЊўњўјўЅў¬ў√ўƒў≈ў∆ў«ў»ў…ў ўЋўћўЌўќўѕў–ў—ў“ў”ў‘ў’ў÷ў„ўЎўўўЏўџў№ўЁўёўяўаўбўвўгўдўеўжўзўиўйўкўлўмўнўоўпўрўсўтўуўфўхўцўчўшўщўъўыўьўэўюЏ°ЏҐЏ£Џ§Џ•Џ¶ЏІЏ®Џ©Џ™ЏЂЏђЏ≠ЏЃЏѓЏ∞Џ±Џ≤Џ≥ЏіЏµЏґЏЈЏЄЏєЏЇЏїЏЉЏљЏЊЏњЏјЏЅЏ¬Џ√ЏƒЏ≈Џ∆Џ«Џ»Џ…Џ ЏЋЏћЏЌЏќЏѕЏ–Џ—Џ“Џ”Џ‘Џ’Џ÷Џ„ЏЎЏўЏЏЏџЏ№ЏЁЏёЏяЏаЏбЏвЏгЏдЏеЏжЏзЏи" +
         "ЏйЏкЏлЏмЏнЏоЏпЏрЏсЏтЏуЏфЏхЏцЏчЏшЏщЏъЏыЏьЏэЏюџ°џҐџ£џ§џ•џ¶џІџ®џ©џ™џЂџђџ≠џЃџѓџ∞џ±џ≤џ≥џіџµџґџЈџЄџєџЇџїџЉџљџЊџњџјџЅџ¬џ√џƒџ≈џ∆џ«џ»џ…џ џЋџћџЌџќџѕџ–џ—џ“џ”џ‘џ’џ÷џ„џЎџўџЏџџџ№џЁџёџяџаџбџвџгџдџеџжџзџиџйџкџлџмџнџоџпџрџсџтџуџфџхџцџчџшџщџъџыџьџэџю№°№Ґ№£№§№•№¶№І№®№©№™№Ђ№ђ№≠№Ѓ№ѓ№∞№±№≤№≥№і№µ№ґ№Ј№Є№є№Ї№ї№Љ№љ№Њ№њ№ј№Ѕ№¬№√№ƒ№≈№∆№«№»№…№ №Ћ№ћ№Ќ№ќ№ѕ№–№—№“№”№‘№’№÷№„№Ў№ў№Џ№џ№№№Ё№ё№я№а№б№в№г№д№е№ж№з№и№й№к№л№м№н№о№п№р№с№т№у№ф№х№ц№ч№ш№щ№ъ№ы№ь№э№юЁ°ЁҐЁ£Ё§Ё•Ё¶ЁІЁ®Ё©Ё™ЁЂЁђЁ≠ЁЃЁѓЁ∞Ё±Ё≤Ё≥ЁіЁµЁґ" +
         "ЁЈЁЄЁєЁЇЁїЁЉЁљЁЊЁњЁјЁЅЁ¬Ё√ЁƒЁ≈Ё∆Ё«Ё»Ё…Ё ЁЋЁћЁЌЁќЁѕЁ–Ё—Ё“Ё”Ё‘Ё’Ё÷Ё„ЁЎЁўЁЏЁџЁ№ЁЁЁёЁяЁаЁбЁвЁгЁдЁеЁжЁзЁиЁйЁкЁлЁмЁнЁоЁпЁрЁсЁтЁуЁфЁхЁцЁчЁшЁщЁъЁыЁьЁэЁюё°ёҐё£ё§ё•ё¶ёІё®ё©ё™ёЂёђё≠ёЃёѓё∞ё±ё≤ё≥ёіёµёґёЈёЄёєёЇёїёЉёљёЊёњёјёЅё¬ё√ёƒё≈ё∆ё«ё»ё…ё ёЋёћёЌёќёѕё–ё—ё“ё”ё‘ё’ё÷ё„ёЎёўёЏёџё№ёЁёёёяёаёбёвёгёдёеёжёзёиёйёкёлёмёнёоёпёрёсётёуёфёхёцёчёшёщёъёыёьёэёюя°яҐя£я§я•я¶яІя®я©я™яЂяђя≠яЃяѓя∞я±я≤я≥яіяµяґяЈяЄяєяЇяїяЉяљяЊяњяјяЅя¬я√яƒя≈я∆я«я»я…я яЋяћяЌяќяѕя–я—я“я”я‘я’я÷я„яЎяўяЏяџя№яЁяёяяяаябявяг" +
         "ядяеяжязяияйякялямяняояпярясятяуяфяхяцячяшящяъяыяьяэяюа°аҐа£а§а•а¶аІа®а©а™аЂађа≠аЃаѓа∞а±а≤а≥аіаµаґаЈаЄаєаЇаїаЉаљаЊањајаЅа¬а√аƒа≈а∆а«а»а…а аЋаћаЌаќаѕа–а—а“а”а‘а’а÷а„аЎаўаЏаџа№аЁаёаяааабавагадаеажазаиайакаламанаоапарасатауафахацачашащаъаыаьаэаюб°бҐб£б§б•б¶бІб®б©б™бЂбђб≠бЃбѓб∞б±б≤б≥бібµбґбЈбЄбєбЇбїбЉбљбЊбњбјбЅб¬б√бƒб≈б∆б«б»б…б бЋбћбЌбќбѕб–б—б“б”б‘б’б÷б„бЎбўбЏбџб№бЁбёбябабббвбгбдбебжбзбибйбкблбмбнбобпбрбсбтбубфбхбцбчбшбщбъбыбьбэбюв°вҐв£в§в•в¶вІв®в©в™вЂвђв≠вЃвѓв∞в±в≤в≥вівµ" +
         "вґвЈвЄвєвЇвївЉвљвЊвњвјвЅв¬в√вƒв≈в∆в«в»в…в вЋвћвЌвќвѕв–в—в“в”в‘в’в÷в„вЎвўвЏвџв№вЁвёвявавбвввгвдвевжвзвивйвквлвмвнвовпврвсвтвувфвхвцвчвшвщвъвывьвэвюг°гҐг£г§г•г¶гІг®г©г™гЂгђг≠гЃгѓг∞г±г≤г≥гігµгґгЈгЄгєгЇгїгЉгљгЊгњгјгЅг¬г√гƒг≈г∆г«г»г…г гЋгћгЌгќгѕг–г—г“г”г‘г’г÷г„гЎгўгЏгџг№гЁгёгягагбгвгггдгегжгзгигйгкглгмгнгогпгргсгтгугфгхгцгчгшгщгъгыгьгэгюд°дҐд£д§д•д¶дІд®д©д™дЂдђд≠дЃдѓд∞д±д≤д≥дідµдґдЈдЄдєдЇдїдЉдљдЊдњдјдЅд¬д√дƒд≈д∆д«д»д…д дЋдћдЌдќдѕд–д—д“д”д‘д’д÷д„дЎдўдЏдџд№дЁдёдядадбдвдгдддедждзди" +
         "дйдкдлдмдндодпдрдсдтдудфдхдцдчдшдщдъдыдьдэдюе°еҐе£е§е•е¶еІе®е©е™еЂеђе≠еЃеѓе∞е±е≤е≥еіеµеґеЈеЄеєеЇеїеЉељеЊењејеЅе¬е√еƒе≈е∆е«е»е…е еЋећеЌеќеѕе–е—е“е”е‘е’е÷е„еЎеўеЏеџе№еЁеёеяеаебевегедееежезеиейекелеменеоепересетеуефехецечешещеъеыеьеэеюж°жҐж£ж§ж•ж¶жІж®ж©ж™жЂжђж≠жЃжѓж∞ж±ж≤ж≥жіжµжґжЈжЄжєжЇжїжЉжљжЊжњжјжЅж¬ж√жƒж≈ж∆ж«ж»ж…ж жЋжћжЌжќжѕж–ж—ж“ж”ж‘ж’ж÷ж„жЎжўжЏжџж№жЁжёжяжажбжвжгжджежжжзжижйжкжлжмжнжожпжржсжтжужфжхжцжчжшжщжъжыжьжэжюз°зҐз£з§з•з¶зІз®з©з™зЂзђз≠зЃзѓз∞з±з≤з≥зізµзґзЈзЄзєзЇзїзЉзљ" +
         "зЊзњзјзЅз¬з√зƒз≈з∆з«з»з…з зЋзћзЌзќзѕз–з—з“з”з‘з’з÷з„зЎзўзЏзџз№зЁзёзязазбзвзгздзезжзззизйзкзлзмзнзозпзрзсзтзузфзхзцзчзшзщзъзызьзэзюи°иҐи£и§и•и¶иІи®и©и™иЂиђи≠иЃиѓи∞и±и≤и≥иіиµиґиЈиЄиєиЇиїиЉиљиЊињијиЅи¬и√иƒи≈и∆и«и»и…и иЋићиЌиќиѕи–и—и“и”и‘и’и÷и„иЎиўиЏиџи№иЁиёияиаибивигидиеижизииийикилиминиоипириситиуифихицичишищиъиыиьиэиюй°йҐй£й§й•й¶йІй®й©й™йЂйђй≠йЃйѓй∞й±й≤й≥йійµйґйЈйЄйєйЇйїйЉйљйЊйњйјйЅй¬й√йƒй≈й∆й«й»й…й йЋйћйЌйќйѕй–й—й“й”й‘й’й÷й„йЎйўйЏйџй№йЁйёйяйайбйвйгйдйейжйзйийййкйлймйнйойпйрйсйтйу" +
         "йфйхйцйчйшйщйъйыйьйэйюк°кҐк£к§к•к¶кІк®к©к™кЂкђк≠кЃкѓк∞к±к≤к≥кікµкґкЈкЄкєкЇкїкЉкљкЊкњкјкЅк¬к√кƒк≈к∆к«к»к…к кЋкћкЌкќкѕк–к—к“к”к‘к’к÷к„кЎкўкЏкџк№кЁкёкякакбквкгкдкекжкзкикйккклкмкнкокпкркскткукфкхкцкчкшкщкъкыкькэкюл°лҐл£л§л•л¶лІл®л©л™лЂлђл≠лЃлѓл∞л±л≤л≥лілµлґлЈлЄлєлЇлїлЉлљлЊлњлјлЅл¬л√лƒл≈л∆л«л»л…л лЋлћлЌлќлѕл–л—л“л”л‘л’л÷л„лЎлўлЏлџл№лЁлёлялалблвлглдлелжлзлилйлклллмлнлолплрлслтлулфлхлцлчлшлщлълыльлэлюм°мҐм£м§м•м¶мІм®м©м™мЂмђм≠мЃмѓм∞м±м≤м≥мімµмґмЈмЄмємЇмїмЉмљмЊмњмјмЅм¬м√мƒм≈м∆м«м»м…м мЋмћмЌ" +
         "мќмѕм–м—м“м”м‘м’м÷м„мЎмўмЏмџм№мЁмёмямамбмвмгмдмемжмзмимймкмлмммнмомпмрмсмтмумфмхмцмчмшмщмъмымьмэмюн°нҐн£н§н•н¶нІн®н©н™нЂнђн≠нЃнѓн∞н±н≤н≥нінµнґнЈнЄнєнЇнїнЉнљнЊнњнјнЅн¬н√нƒн≈н∆н«н»н…н нЋнћнЌнќнѕн–н—н“н”н‘н’н÷н„нЎнўнЏнџн№нЁнёнянанбнвнгндненжнзнинйнкнлнмнннонпнрнснтнунфнхнцнчншнщнъныньнэнюо°оҐо£о§о•о¶оІо®о©о™оЂођо≠оЃоѓо∞о±о≤о≥оіоµоґоЈоЄоєоЇоїоЉољоЊоњојоЅо¬о√оƒо≈о∆о«о»о…о оЋоћоЌоќоѕо–о—о“о”о‘о’о÷о„оЎоўоЏоџо№оЁоёояоаобовогодоеожозоиойоколомонооопоросотоуофохоцочошощоъоыоьоэоюп°пҐп£п§п•п¶пІп®п©п™" +
         "пЂпђп≠пЃпѓп∞п±п≤п≥піпµпґпЈпЄпєпЇпїпЉпљпЊпњпјпЅп¬п√пƒп≈п∆п«п»п…п пЋпћпЌпќпѕп–п—п“п”п‘п’п÷п„пЎпўпЏпџп№пЁпёпяпапбпвпгпдпепжпзпипйпкплпмпнпопппрпсптпупфпхпцпчпшпщпъпыпьпэпюр°рҐр£р§р•р¶рІр®р©р™рЂрђр≠рЃрѓр∞р±р≤р≥рірµрґрЈрЄрєрЇрїрЉрљрЊрњрјрЅр¬р√рƒр≈р∆р«р»р…р рЋрћрЌрќрѕр–р—р“р”р‘р’р÷р„рЎрўрЏрџр№рЁрёрярарбрвргрдрержрзрирйркрлрмрнрорпрррсртрурфрхрцрчршрщрърырьрэрюс°сҐс£с§с•с¶сІс®с©с™сЂсђс≠сЃсѓс∞с±с≤с≥сісµсґсЈсЄсєсЇсїсЉсљсЊсњсјсЅс¬с√сƒс≈с∆с«с…с сЋсћсЌсќсѕс–с—с“с”с‘с’с÷с„сЎсўсЏсџс№сЁсёсясасвсгсдсесжсз" +
         "сисйскслсмснсоспсрссстсусфсхсцсчсшсщсъсысьсэсют°тҐт£т§т•т¶тІт®т©т™тЂтђт≠тЃтѓт∞т±т≤т≥тітµтґтЈтЄтєтЇтїтЉтљтЊтњтјтЅт¬т√тƒт≈т∆т«т»т…т тЋтћтЌтќтѕт–т—т“т”т‘т’т÷т„тЎтўтЏтџт№тЁтётятатбтвтгтдтетжтзтитйтктлтмтнтотптртстттутфтхтцтчтштщтътытьтэтюу°уҐу£у§у•у¶уІу®у©у™уЂуђу≠уЃуѓу∞у±у≤у≥уіуµуґуЈуЄуєуЇуїуЉуљуЊуњујуЅу¬у√уƒу≈у∆у«у»у…у уЋућуЌуќуѕу–у—у“у”у‘у’у÷у„уЎуўуЏуџу№уЁуёуяуаубувугудуеужузуиуйукулумунуоупурусутуууфухуцучушущуъуыуьуэуюф°фҐф£ф§ф•ф¶фІф®ф©ф™фЂфђф≠фЃфѓф∞ф±ф≤ф≥фіфµфґфЈфЄфєфЇфїфЉфљфЊфњфјфЅф¬ф√фƒф≈ф∆ф«" +
         "ф»ф…ф фЋфћфЌфќфѕф–ф—ф“ф”ф‘ф’ф÷ф„фЎфўфЏфџф№фЁфёфяфафбфвфгфдфефжфзфифйфкфлфмфнфофпфрфсфтфуфффхфцфчфшфщфъфыфьфэфюх°хҐх£х§х•х¶хІх®х©х™хЂхђх≠хЃхѓх∞х±х≤х≥хіхµхґхЈхЄхєхЇхїхЉхљхЊхњхјхЅх¬х√хƒх≈х∆х«х»х…х хЋхћхЌхќхѕх–х—х“х”х‘х’х÷х„хЎхўхЏхџх№хЁхёхяхахбхвхгхдхехжхзхихйхкхлхмхнхохпхрхсхтхухфхххцхчхшхщхъхыхьхэхюц°цҐц£ц§ц•ц¶цІц®ц©ц™цЂцђц≠цЃцѓц∞ц±ц≤ц≥ціцµцґцЈцЄцєцЇцїцЉцљцЊцњцјцЅц¬ц√цƒц≈ц∆ц«ц»ц…ц цЋцћцЌцќцѕц–ц—ц“ц”ц‘ц’ц÷ц„цЎцўцЏцџц№цЁцёцяцацбцвцгцдцецжцзцицйцкцлцмцнцоцпцрцсцтцуцфцхцццчцшцщцъцыцьцэцюч°чҐч£ч§ч•ч¶чІ" +
         "ч®ч©ч™чЂчђч≠чЃчѓч∞ч±ч≤ч≥чічµчґчЈчЄчєчЇчїчЉчљчЊчњчјчЅч¬ч√чƒч≈ч∆ч«ч»ч…ч чЋчћчЌчќчѕч–ч—ч“ч”ч‘ч’ч÷ч„чЎчўчЏчџч№чЁчёчячачбчвчгчдчечжчзчичйчкчлчмчнчочпчрчсчтчучфчхчцчччшчщчъчычьчэчю";

            string return_py = "";
            byte[] array = new byte[2];

            for (int i = 0; i < hz.Length; i++)
            {
                array = System.Text.Encoding.Default.GetBytes(hz[i].ToString());

                //Ј«ЇЇ„÷
                if (array[0] < 176)
                {
                    return_py += hz[i];
                }
                //“їЉґЇЇ„÷
                else if (array[0] >= 176 && array[0] <= 215)
                {

                    if (hz[i].ToString().CompareTo("‘—") >= 0)
                        return_py += "z";
                    else if (hz[i].ToString().CompareTo("—є") >= 0)
                        return_py += "y";
                    else if (hz[i].ToString().CompareTo("ќф") >= 0)
                        return_py += "x";
                    else if (hz[i].ToString().CompareTo("Ќџ") >= 0)
                        return_py += "w";
                    else if (hz[i].ToString().CompareTo("Ћъ") >= 0)
                        return_py += "t";
                    else if (hz[i].ToString().CompareTo("»ц") >= 0)
                        return_py += "s";
                    else if (hz[i].ToString().CompareTo("»ї") >= 0)
                        return_py += "r";
                    else if (hz[i].ToString().CompareTo("∆Џ") >= 0)
                        return_py += "q";
                    else if (hz[i].ToString().CompareTo("≈Њ") >= 0)
                        return_py += "p";
                    else if (hz[i].ToString().CompareTo("≈ґ") >= 0)
                        return_py += "o";
                    else if (hz[i].ToString().CompareTo("ƒ√") >= 0)
                        return_py += "n";
                    else if (hz[i].ToString().CompareTo("¬и") >= 0)
                        return_py += "m";
                    else if (hz[i].ToString().CompareTo("јђ") >= 0)
                        return_py += "l";
                    else if (hz[i].ToString().CompareTo("њ¶") >= 0)
                        return_py += "k";
                    else if (hz[i].ToString().CompareTo("їч") >= 0)
                        return_py += "j";
                    else if (hz[i].ToString().CompareTo("єю") >= 0)
                        return_py += "h";
                    else if (hz[i].ToString().CompareTo("ЄЅ") >= 0)
                        return_py += "g";
                    else if (hz[i].ToString().CompareTo("ЈҐ") >= 0)
                        return_py += "f";
                    else if (hz[i].ToString().CompareTo("ґк") >= 0)
                        return_py += "e";
                    else if (hz[i].ToString().CompareTo("іо") >= 0)
                        return_py += "d";
                    else if (hz[i].ToString().CompareTo("≤Ѕ") >= 0)
                        return_py += "c";
                    else if (hz[i].ToString().CompareTo("∞≈") >= 0)
                        return_py += "b";
                    else if (hz[i].ToString().CompareTo("∞°") >= 0)
                        return_py += "a";
                }
                //ґюЉґЇЇ„÷
                else if (array[0] >= 215)
                {
                    return_py += ls_second_eng.Substring(ls_second_ch.IndexOf(hz[i].ToString(), 0), 1);
                }
            }
            return return_py.ToUpper();
        }

        /// <summary>
        /// »°ЇЇ„÷∆і“фµƒ „„÷ƒЄ
        /// </summary>
        /// <param name="UnName">ЇЇ„÷</param>
        /// <returns> „„÷ƒЄ</returns>
        public static string GetCodstring(string UnName)
        {
            int i = 0;
            ushort key = 0;
            string strResult = string.Empty;

            Encoding unicode = Encoding.Unicode;
            Encoding gbk = Encoding.GetEncoding(936);
            byte[] unicodeBytes = unicode.GetBytes(UnName);
            byte[] gbkBytes = Encoding.Convert(unicode, gbk, unicodeBytes);
            while (i < gbkBytes.Length)
            {
                if (gbkBytes[i] <= 127)
                {
                    strResult = strResult + (char)gbkBytes[i];
                    i++;
                }
                #region …ъ≥…ЇЇ„÷∆і“фЉт¬л,»°∆і“ф „„÷ƒЄ
                else
                {
                    key = (ushort)(gbkBytes[i] * 256 + gbkBytes[i + 1]);
                    if (key >= '\uB0A1' && key <= '\uB0C4')
                    {
                        strResult = strResult + "A";
                    }
                    else if (key >= '\uB0C5' && key <= '\uB2C0')
                    {
                        strResult = strResult + "B";
                    }
                    else if (key >= '\uB2C1' && key <= '\uB4ED')
                    {
                        strResult = strResult + "C";
                    }
                    else if (key >= '\uB4EE' && key <= '\uB6E9')
                    {
                        strResult = strResult + "D";
                    }
                    else if (key >= '\uB6EA' && key <= '\uB7A1')
                    {
                        strResult = strResult + "E";
                    }
                    else if (key >= '\uB7A2' && key <= '\uB8C0')
                    {
                        strResult = strResult + "F";
                    }
                    else if (key >= '\uB8C1' && key <= '\uB9FD')
                    {
                        strResult = strResult + "G";
                    }
                    else if (key >= '\uB9FE' && key <= '\uBBF6')
                    {
                        strResult = strResult + "H";
                    }
                    else if (key >= '\uBBF7' && key <= '\uBFA5')
                    {
                        strResult = strResult + "J";
                    }
                    else if (key >= '\uBFA6' && key <= '\uC0AB')
                    {
                        strResult = strResult + "K";
                    }
                    else if (key >= '\uC0AC' && key <= '\uC2E7')
                    {
                        strResult = strResult + "L";
                    }
                    else if (key >= '\uC2E8' && key <= '\uC4C2')
                    {
                        strResult = strResult + "M";
                    }
                    else if (key >= '\uC4C3' && key <= '\uC5B5')
                    {
                        strResult = strResult + "N";
                    }
                    else if (key >= '\uC5B6' && key <= '\uC5BD')
                    {
                        strResult = strResult + "O";
                    }
                    else if (key >= '\uC5BE' && key <= '\uC6D9')
                    {
                        strResult = strResult + "P";
                    }
                    else if (key >= '\uC6DA' && key <= '\uC8BA')
                    {
                        strResult = strResult + "Q";
                    }
                    else if (key >= '\uC8BB' && key <= '\uC8F5')
                    {
                        strResult = strResult + "R";
                    }
                    else if (key >= '\uC8F6' && key <= '\uCBF9')
                    {
                        strResult = strResult + "S";
                    }
                    else if (key >= '\uCBFA' && key <= '\uCDD9')
                    {
                        strResult = strResult + "T";
                    }
                    else if (key >= '\uCDDA' && key <= '\uCEF3')
                    {
                        strResult = strResult + "W";
                    }
                    else if (key >= '\uCEF4' && key <= '\uD188')
                    {
                        strResult = strResult + "X";
                    }
                    else if (key >= '\uD1B9' && key <= '\uD4D0')
                    {
                        strResult = strResult + "Y";
                    }
                    else if (key >= '\uD4D1' && key <= '\uD7F9')
                    {
                        strResult = strResult + "Z";
                    }
                    else
                    {
                        strResult = strResult + "?";
                    }
                    i = i + 2;
                }
                #endregion
            }
            return strResult;
        }
    }
}