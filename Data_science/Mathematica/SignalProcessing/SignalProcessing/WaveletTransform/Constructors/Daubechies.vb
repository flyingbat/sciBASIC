﻿#Region "Microsoft.VisualBasic::867a991f956604f21f7eb1ea8bc0f4c3, Data_science\Mathematica\SignalProcessing\SignalProcessing\WaveletTransform\Constructors\Daubechies.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.



    ' /********************************************************************************/

    ' Summaries:

    '     Class Daubechies
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: CreateAllDaubechies
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace WaveletTransform

    Public Class Daubechies : Inherits WaveletConstructor

        Public Sub New()
            MyBase.New(wavelets:=CreateAllDaubechies)
        End Sub

        Public Shared Iterator Function CreateAllDaubechies() As IEnumerable(Of Wavelet)
            Yield New Wavelet("Daubechies_1", {
            0.7071067812, 0.7071067812
        })
            Yield New Wavelet("Daubechies_2", {
            0.48296291314469, 0.836516303737469, 0.224143868041857, -0.129409522550921
        })
            Yield New Wavelet("Daubechies_3", {
            0.332670552950957, 0.806891509313339, 0.459877502119331, -0.135011020010391, -0.0854412738822415, 0.0352262918821007
        })
            Yield New Wavelet("Daubechies_4", {
            0.230377813308897, 0.714846570552916, 0.630880767929859, -0.0279837694168599, -0.187034811719093, 0.0308413818355608,
            0.0328830116668852, -0.010597401785069
        })
            Yield New Wavelet("Daubechies_5", {
            0.160102397974193, 0.60382926979719, 0.724308528437773, 0.138428145901321, -0.242294887066382, -0.0322448695846384,
            0.0775714938400457, -0.00624149021279827, -0.012580751999082, 0.00333572528547377
        })
            Yield New Wavelet("Daubechies_6", {
            0.111540743350109, 0.494623890398453, 0.751133908021095, 0.315250351709198, -0.22626469396544, -0.129766867567262,
            0.097501605587323, 0.0275228655303057, -0.031582039317486, 0.000553842201161496, 0.00477725751094551,
            -0.00107730108530848
        })
            Yield New Wavelet("Daubechies_7", {
            0.0778520540850092, 0.396539319481917, 0.729132090846235, 0.469782287405193, -0.143906003928565, -0.224036184993875,
            0.0713092192668303, 0.0806126091510831, -0.0380299369350144, -0.0165745416306669, 0.0125509985560998, 0.000429577972921367,
            -0.00180164070404749, 0.00035371379997452
        })
            Yield New Wavelet("Daubechies_8", {
            0.054415842243104, 0.3128715909143, 0.67563073629729, 0.585354683654207, -0.0158291052563493, -0.284015542961547,
            0.000472484573913283, 0.128747426620478, -0.0173693010018075, -0.0440882539307948, 0.0139810279173983, 0.00874609404740578,
            -0.00487035299345157, -0.000391740373376947, 0.000675449406450569, -0.00011747678412477
        })
            Yield New Wavelet("Daubechies_9", {
            0.0380779473638783, 0.24383467461259, 0.604823123690111, 0.657288078051301, 0.133197385825008, -0.293273783279175,
            -0.0968407832229765, 0.148540749338106, 0.0307256814793334, -0.06763282906133, 0.000250947114831452, 0.0223616621236791,
            -0.0047232047577514, -0.00428150368246343, 0.00184764688305623, 0.000230385763523196, -0.00025196318894271,
            0.0000393473203162716
        })
            Yield New Wavelet("Daubechies_10", {
            0.0266700579005556, 0.188176800077691, 0.527201188931726, 0.688459039453604, 0.281172343660577, -0.249846424327315,
            -0.195946274377377, 0.127369340335793, 0.0930573646035723, -0.0713941471663971, -0.0294575368218758, 0.033212674059341,
            0.00360655356695617, -0.0107331754833306, 0.0013953517470529, 0.00199240529518506, -0.000685856694959712, -0.000116466855129285,
            0.0000935886703200696, -0.0000132642028945212
        })
            Yield New Wavelet("Daubechies_11", {
            0.0186942977614711, 0.144067021150625, 0.449899764356045, 0.685686774916201, 0.411964368947907, -0.16227524502749,
            -0.274230846817947, 0.0660435881966832, 0.149812012466378, -0.0464799551166842, -0.0664387856950252, 0.0313350902190461,
            0.0208409043601811, -0.0153648209062016, -0.00334085887301445, 0.00492841765605904, -0.000308592858815143, -0.000893023250666265,
            0.000249152523552823, 0.0000544390746993685, -0.000034634984186985, 0.00000449427427723651
        })
            Yield New Wavelet("Daubechies_12", {
            0.0131122579572295, 0.109566272821185, 0.377355135214213, 0.657198722579307, 0.515886478427816, -0.0447638856537746,
            -0.316178453752786, -0.0237792572560697, 0.18247860592758, 0.00535956967435215, -0.0964321200965071, 0.0108491302558222,
            0.0415462774950844, -0.0122186490697483, -0.0128408251983007, 0.00671149900879551, 0.00224860724099524, -0.00217950361862776,
            0.0000065451282125096, 0.000388653062820931, -0.0000885041092082043, -0.0000242415457570308, 0.0000127769522193798,
            -0.00000152907175806851
        })
            Yield New Wavelet("Daubechies_13", {
            0.00920213353896237, 0.0828612438729028, 0.311996322160438, 0.611055851158788, 0.588889570431219, 0.0869857261796472,
            -0.314972907711389, -0.124576730750815, 0.17947607942934, 0.0729489336567772, -0.105807618187934, -0.0264884064753437,
            0.0561394771002834, 0.00237997225405908, -0.0238314207103237, 0.00392394144879742, 0.00725558940161757, -0.00276191123465686,
            -0.0013156739118923, 0.000932326130867263, 0.0000492515251262895, -0.000165128988556505, 0.0000306785375793255,
            0.0000104419305714081, -0.00000470041647936087, 0.000000522003509845486
        })
            Yield New Wavelet("Daubechies_14", {
            0.00646115346008795, 0.0623647588493989, 0.254850267792621, 0.554305617940894, 0.631187849104857, 0.218670687758907,
            -0.271688552278748, -0.218033529993276, 0.138395213864807, 0.139989016584461, -0.0867484115681697, -0.0715489555040461,
            0.055237126259216, 0.0269814083079129, -0.0301853515403906, -0.00561504953035696, 0.0127894932663334, -0.000746218989268385,
            -0.00384963886802219, 0.00106169108560676, 0.000708021154235528, -0.000386831947312955, -0.0000417772457703726, 0.0000687550425269751,
            -0.0000103372091845708, -0.00000438970490178139, 0.00000172499467536781, -0.000000178713996831136
        })
            Yield New Wavelet("Daubechies_15", {
            0.0045385373615789, 0.0467433948927663, 0.206023863986996, 0.49263177170814, 0.645813140357424, 0.339002535454732,
            -0.193204139609145, -0.288882596566966, 0.0652829528487728, 0.190146714007123, -0.0396661765557909, -0.111120936037232,
            0.0338771439235077, 0.0547805505845076, -0.02576700732844, -0.0208100501696931, 0.0150839180278359, 0.00510100036040754,
            -0.00648773456031575, -0.000241756490761624, 0.00194332398038221, -0.000373482354137617, -0.000359565244362469, 0.0001558964899206,
            0.0000257926991553189, -0.0000281332962660478, 0.00000336298718173758, 0.00000181127040794058, -0.000000631688232588166,
            0.0000000613335991330575
        })
            Yield New Wavelet("Daubechies_16", {
            0.00318922092534774, 0.0349077143236733, 0.165064283488853, 0.430312722846004, 0.637356332083789, 0.440290256886357,
            -0.0897510894024896, -0.327063310527918, -0.0279182081330283, 0.211190693947104, 0.027340263752716, -0.13238830556381,
            -0.00623972275247487, 0.0759242360442763, -0.00758897436885774, -0.0368883976917301, 0.010297659640956, 0.0139937688598287,
            -0.00699001456341392, -0.00364427962149839, 0.00312802338120627, 0.000407896980849713, -0.000941021749359568, 0.000114241520038722,
            0.000174787245225338, -0.0000610359662141094, -0.0000139456689882089, 0.0000113366086612763, -0.00000104357134231161,
            -0.000000736365678545121, 0.000000230878408685755, -0.0000000210933963010074
        })
            Yield New Wavelet("Daubechies_17", {
            0.00224180700103731, 0.025985393703606, 0.131214903307824, 0.370350724152641, 0.610996615684623, 0.518315764056938,
            0.0273149704032936, -0.328320748363962, -0.126599752215883, 0.197310589565011, 0.10113548917747, -0.126815691778286,
            -0.0570914196316769, 0.0811059866541609, 0.0223123361781038, -0.0469224383892697, -0.00327095553581929, 0.0227336765839463,
            -0.00304298998135464, -0.00860292152032286, 0.00296799669152609, 0.00230120524215355, -0.00143684530480298, -0.000328132519409838,
            0.000439465427768644, -0.0000256101095665485, -0.0000820480320245339, 0.000023186813798746, 0.00000699060098507675,
            -0.00000450594247722299, 0.000000301654960999456, 0.000000295770093331686, -0.0000000842394844600268, 0.00000000726749296856161
        })
            Yield New Wavelet("Daubechies_18", {
            0.00157631021844076, 0.0192885317241464, 0.103588465822424, 0.314678941337032, 0.571826807766607, 0.571801654888651,
            0.147223111969928, -0.293654040736559, -0.216480934005143, 0.149533975565378, 0.167081312763257, -0.0923318841508463,
            -0.106752246659828, 0.0648872162119054, 0.0570512477385369, -0.0445261419029823, -0.02373321039586, 0.0266707059264706,
            0.00626216795430571, -0.013051480946612, 0.000118630033858117, 0.00494334360546674, -0.0011187326669925, -0.00134059629833611,
            0.000628465682965146, 0.000213581561910341, -0.000198648552311748, -0.000000153591712353472, 0.0000374123788074004, -0.0000085206025374467,
            -0.00000333263447888582, 0.00000176871298362762, -0.0000000769163268988518, -0.000000117609876702823, 0.0000000306883586304517,
            -0.0000000025079344549486
        })
            Yield New Wavelet("Daubechies_19", {
            0.00110866976318171, 0.0142810984507644, 0.0812781132654596, 0.264388431740897, 0.524436377464655, 0.601704549127538,
            0.260894952651039, -0.228091394215483, -0.285838631755826, 0.0746522697081033, 0.212349743306278, -0.0335185419023029,
            -0.142785695038737, 0.0275843506256287, 0.0869067555558122, -0.026501236250123, -0.0456742262772309, 0.021623767409585,
            0.0193755498891761, -0.0139883886785351, -0.00586692228101217, 0.00704074736710524, 0.000768954359257548, -0.00268755180070158,
            0.000341808653458596, 0.000735802520505435, -0.000260676135678628, -0.000124600791734159, 0.0000871127046721992, 0.00000510595048707389,
            -0.0000166401762971549, 0.00000301096431629653, 0.00000153193147669119, -0.000000686275565776914, 0.0000000144708829879784,
            0.000000046369377757826, -0.0000000111640206703583, 0.000000000866684883899762
        })
            Yield New Wavelet("Daubechies_20", {
            0.000779953613666846, 0.0105493946249504, 0.0634237804590815, 0.219942113551397, 0.472696185310902, 0.610493238938594,
            0.361502298739331, -0.139212088011484, -0.326786800434035, -0.016727088309077, 0.228291050819916, 0.0398502464577712,
            -0.155458750707268, -0.0247168273386136, 0.102291719174443, 0.00563224685730744, -0.0617228996246805, 0.00587468181181183,
            0.0322942995307696, -0.00878932492390156, -0.0138105261371519, 0.00672162730225946, 0.00442054238704579, -0.00358149425960962,
            -0.000831562172822557, 0.00139255961932314, -0.0000534975984399769, -0.000385104748699218, 0.000101532889736703, 0.0000677428082837773,
            -0.0000371058618339471, -0.000004376143862184, 0.00000724124828767362, -0.00000101199401001889, -0.000000684707959700056,
            0.000000263392422627, 0.000000000201432202355051, -0.000000018148432482997, 0.00000000405612705555183, -0.000000000299883648961932
        })
        End Function
    End Class
End Namespace
