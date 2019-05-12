using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

internal sealed class \uE002
{
	private static string \uE000;

	private static \uE002.\uE003 \uE001;

	static \uE002()
	{
		Assembly executingAssembly = Assembly.GetExecutingAssembly();
		\uE002.\uE001 = new \uE002.\uE003(\uE002.\uE001);
		Stream uE = \uE00A.\uE000(executingAssembly.GetManifestResourceStream(\uE002.\uE001(~(~(-(-(-(-(~(-(-515070395 ^ 1482159112)))))))) ^ 1744887818 ^ -786714042)));
		\uE002.\uE000 = new \uE002.\uE009().\uE001(uE);
	}

	public static string \uE000(int \uE000)
	{
		return (string)((Hashtable)AppDomain.CurrentDomain.GetData(\uE002.\uE000))[\uE000];
	}

	public static string \uE001(int \uE000)
	{
		char[] array = "öûÂÚË".ToCharArray();
		int num = array.Length;
		while ((num -= -(~(-(~(~(-(~(-(-(-(--1))))))))))) >= ~(~(~(-(~(~(-(~(-(~-341975827)))) ^ -341975826))))))
		{
			array[num] = (char)((int)array[num] ^ ~(-(-(-(-(-(~(~(~516714473 ^ 2084118493) ^ 453632380)) ^ 711077178 ^ -1402911997))))) ^ \uE000);
		}
		return new string(array);
	}

	private delegate string \uE003(int \uE000);

	private sealed class \uE009
	{
		public MethodBuilder \uE000(TypeBuilder \uE000)
		{
			MethodAttributes methodAttributes = -(~(-(~(-(~(~(MethodAttributes)1078287594)) ^ (MethodAttributes)(-54643605)) ^ (MethodAttributes)1124374505)));
			string[] array;
			MethodInfo method;
			MethodInfo method2;
			MethodInfo method3;
			MethodBuilder methodBuilder;
			MethodInfo method4;
			MethodInfo method5;
			MethodInfo method6;
			MethodInfo method7;
			MethodInfo method8;
			MethodInfo method9;
			MethodInfo method10;
			MethodInfo method11;
			ConstructorInfo constructor;
			MethodInfo method12;
			ConstructorInfo constructor2;
			MethodInfo method13;
			ConstructorInfo constructor3;
			MethodInfo method14;
			MethodInfo method15;
			for (;;)
			{
				int num = ~(-(-(--1999460469)) ^ -1579764995 ^ -421180699) ^ -807313432;
				for (;;)
				{
					num ^= (-(~(-(-(-526670098 ^ 153862340)) ^ 942816526)) ^ -1782999495 ^ 1144769829);
					switch (num - (-(-(-(~(-(-(~(~-2096484770 ^ -207042640) ^ -1591536044 ^ -1129351523)))))) ^ 1831766289))
					{
					case 0:
					{
						BindingFlags bindingFlags;
						method = typeof(Stream).GetMethod(array[~(~(-(-(-(~(~-2127430392 ^ 1137707218 ^ -2094303352)) ^ -1012389825))) ^ 813607503) ^ 1306787795], bindingFlags, null, new Type[-(-(~(-(~(-1878014390 ^ -1448500116))))) ^ -967205416], null);
						BindingFlags bindingFlags2;
						method2 = typeof(AppDomain).GetMethod(array[-(-(~(~(-(~684974176))) ^ -639520035 ^ -248360270))], bindingFlags2, null, new Type[~(~(-(-1517601290 ^ 2026205070) ^ 1934755636) ^ 1373874876)], null);
						Type typeFromHandle = typeof(AppDomain);
						string name = array[-(~(~-437112943) ^ 1711373355 ^ 2081205323)];
						BindingFlags bindingAttr = bindingFlags;
						Binder binder = null;
						Type[] array2 = new Type[-(~(~(~(--591407445 ^ 857869225 ^ 2122023093) ^ -1393798357 ^ 2073263632 ^ -492458614)) ^ 1539515129)];
						array2[~(-(-(~(~(-(-(~(-(-1981335813 ^ -101136518 ^ -670226379 ^ 15161729)))))) ^ 1460305867)))] = typeof(string);
						array2[-(-(-(~(~(--2023899005) ^ -2023899006))))] = typeof(object);
						method3 = typeFromHandle.GetMethod(name, bindingAttr, binder, array2, null);
						methodBuilder.SetReturnType(typeof(string));
						num = ~(-(-(~(-(-437731698 ^ 1941635365) ^ 1382026900 ^ 656108136))) ^ 483861724);
						continue;
					}
					case 1:
					{
						byte[] array3;
						array3[~(~(~(-1120981176)) ^ 1370604215 ^ -357841461 ^ -104146966)] = (byte)((int)array3[~(~(-1106008569)) ^ -1592512028 ^ 371260772 ^ 1678783651 ^ 1832345093] ^ ~(-(~(~(-(-(-(-(~(~(-433698963))))))))) ^ -433698967));
						array3[~(-(553318975 ^ 323531870)) ^ 262851713 ^ -920765936 ^ -184108845] = (byte)((int)array3[-(-(-(-(~(~(-(1819841 ^ -1701974617 ^ 1609726471) ^ -1552763388)))))) ^ -1383755706 ^ 879528703] ^ ~(-(~(~(~(~-474581138) ^ -1075202161) ^ 1268602744 ^ -550834234 ^ -1525601370) ^ 1845056473)));
						array3[-(-(~(-36)))] = (byte)((int)array3[~(-(~(~(-(~-1431261701 ^ -1431261736)))))] ^ (-(~(-(~(~(~(-(~(--423623141) ^ -942246027) ^ 1507455607)))))) ^ -2026865456));
						array3[-(-(-(-(-(1134024589 ^ -66225225) ^ 518739173) ^ -1683347030)) ^ -987484501)] = (byte)((int)array3[-(-(-(~-1199376223) ^ -1199376250))] ^ (-(-(-(~(~(~(-(-(545971537 ^ -1446198645)))))))) ^ -1991893018));
						num = -(~(~(~(~(-(~(~(~(-(~1893909296)) ^ -1893909322))))))));
						continue;
					}
					case 2:
					{
						byte[] array3;
						array3[-(~(~(~(~(-(~(1997301046 ^ 1710556082)) ^ -318334638)))))] = (byte)((int)array3[-(-(~(-(~(-(~-1550986867)))) ^ -105223190)) ^ -1513601613] ^ -(~(~(~(-(~(-(~(-(-2121060123)))) ^ 2121060125))))));
						array3[-(-(-(-(-(1806911879 ^ 709043443) ^ -1982539230)) ^ 145088038 ^ 1065260706))] = (byte)((int)array3[-(~(-(~(-731180711 ^ -889026554) ^ 104330831 ^ -750163205 ^ 2137220728) ^ -1250663491))] ^ -(-(-(~(-(~(-(~200921911 ^ 1538143832 ^ -2048609546) ^ -1884988791))))) ^ 1511394656));
						array3[-(~(~(~(~1705826313) ^ -1240767393 ^ -1369516117) ^ 402642773)) ^ 830066296 ^ 2044251388 ^ -581418505] = (byte)((int)array3[-(-(66544789 ^ -267111927 ^ -203204937))] ^ -(~(~(~(-(-(~(-(-(~2103791053) ^ 1062970194) ^ -707188490) ^ -1746418132)))))));
						array3[~(-(~(~1317019753 ^ 1332305665))) ^ 32079691] = (byte)((int)array3[-(-(-(--71198364) ^ -71198392))] ^ (-(~(-769886823 ^ 1306579051)) ^ -416259985 ^ -101689791 ^ -2126736454));
						num = -(~(~(-(-(-(~(~(-(-(-1249947772 ^ 221997451)))))))) ^ -1203493366));
						continue;
					}
					case 3:
					{
						byte[] array3;
						array3[~(-(-(-(-(-(--1396230242) ^ 50695002 ^ -1200693947 ^ -865710192 ^ 607506883)))))] = (byte)((int)array3[~(~(-(-(-(-(271783073 ^ 1798036914)))) ^ 787360074 ^ 1442242676))] ^ (-(-(~(~(-(-(-(-1986897265 ^ -876131372) ^ 676495449)) ^ -424093167 ^ 2087964365)))) ^ 255031809));
						array3[~(-(-(-559821882) ^ 559821845))] = (byte)((int)array3[-(-(-(-2098544540)) ^ -2007146267) ^ -179806383] ^ (-(~(-241957467 ^ -384303369 ^ 2026361536)) ^ 1615079881));
						array3[~(-(-(-(~(~(-406425253 ^ -1913823240))) ^ 1781214861)))] = (byte)((int)array3[-(-(-(1227181209 ^ 1633248269)) ^ -679225531)] ^ ~(~(-(~(-(-(-(~(~(--1749820290 ^ 1654274012)))) ^ -181802131))))));
						array3[-(-(-(~(-(~(-60138597 ^ -1478885779)))))) ^ 1848740290 ^ 897723402] = (byte)((int)array3[~(-(~(--2118258913 ^ 1172435162) ^ -50570729 ^ -751227366 ^ -342266888))] ^ -(~(~(-(~(-(-(-1366938834 ^ -1526293350)) ^ -192984986))))));
						num = (-(-(-(501274310 ^ -1191191265) ^ 763453380) ^ 1116740975) ^ 905033463);
						continue;
					}
					case 4:
					{
						Type typeFromHandle2 = typeof(string);
						string name2 = array[-(~(~(~(789502622 ^ 789502616))))];
						BindingFlags bindingFlags;
						BindingFlags bindingAttr2 = bindingFlags;
						Binder binder2 = null;
						Type[] array4 = new Type[-(~(~(~(-(~-1237658236)) ^ -1237658235)))];
						array4[~(-(~(~(-(~(-(-(~-1422539168) ^ -1422539167)))))))] = typeof(string);
						method4 = typeFromHandle2.GetMethod(name2, bindingAttr2, binder2, array4, null);
						for (;;)
						{
							int num2 = ~(-(-(-(-(-(~(950290351 ^ 1356016535)))) ^ 1877393584))) ^ -126987402;
							for (;;)
							{
								switch ((num2 ^ ~(~(-(-(~(-(-(-(2125231384 ^ -2097518122) ^ -1157648171) ^ 297473876 ^ 1460845428))))))) - (~(-(-1327345699 ^ 933999160 ^ 762130424)) ^ -1440427478))
								{
								case 0:
									num = ~(~(-(-(~(~-1241888201) ^ -1241888180))));
									num2 = ~(~(-861787041 ^ -2084304122 ^ 626572437 ^ 1782488014));
									continue;
								case 1:
								{
									Type type;
									method5 = type.GetMethod(array[-(-(~(-(-(-(~(-(~(358487337 ^ -210849528))))) ^ 901899555))) ^ -823414491) ^ -488304685], bindingFlags, null, new Type[~(~(-(-(~(~(~(--430034893 ^ 800234690 ^ 1114596111)) ^ 1097054227) ^ -891258900))))], null);
									num2 = -(~(~(~(-(~(~(-(~(-(618124140 ^ 618124141))))))))));
									continue;
								}
								case 2:
								{
									Type type2 = Type.GetType(array[-(-(~(~(-(-(-(-(~(-1321677651 ^ -944427474)) ^ -1275013669) ^ 1030932661))))))]);
									string name3 = array[-(-(-(~(~615042424)) ^ -1751231597 ^ 1288266003))];
									BindingFlags bindingFlags2;
									BindingFlags bindingAttr3 = bindingFlags2;
									Binder binder3 = null;
									Type[] array5 = new Type[-(~(-(913161656 ^ 1451355581 ^ 1481287022) ^ -950465899))];
									array5[~(~-437208928 ^ -1618093645 ^ -378373956 ^ -1935286940 ^ -1264656313) ^ -1422646132] = typeof(int);
									method6 = type2.GetMethod(name3, bindingAttr3, binder3, array5, null);
									num2 = -(-(~(~(~(-(~(~(-2026837679 ^ -586400422))))) ^ 266264058) ^ 1440877055));
									continue;
								}
								case 3:
									method7 = typeof(Stream).GetMethod(array[~(~(-(~(-(-(~-837790814 ^ -77218897)))))) ^ -896919559], bindingFlags, null, new Type[~(-(~(~(-(--2056206485)) ^ -1077606827 ^ 452839359 ^ 1776152231))) ^ 1234561061], null);
									num2 = (-(~(~(~(~(-(~(-(~-1681225846 ^ 705900067)))))))) ^ 1311135323);
									continue;
								case 4:
									goto IL_986;
								}
								break;
							}
						}
						IL_986:
						continue;
					}
					case 5:
					{
						byte[] array3;
						array3[-(~(~(-(--71429773))) ^ -2095852490) ^ -1020885125 ^ 1148501465] = (byte)((int)array3[~(-(~(-(-(-(-(-878586302) ^ -1934882303) ^ -873301881) ^ 210328359)))) ^ 2139684866] ^ ~(-(-(-(~(~(~(~(~(-1292798723) ^ -1887625144 ^ 1326381759) ^ -1921166899))))))));
						array3[~(-(-(~(-1145907602 ^ -1995788174) ^ -420959920 ^ 1876830355)) ^ -1162419136 ^ 1609211159) ^ 1590861458] = (byte)((int)array3[-(~(-1305282573 ^ 6830462)) ^ -119548259 ^ 1250236425] ^ (~(~(~(~(-(-835276296 ^ -2009702731 ^ -21415706))))) ^ -566907280 ^ -1720482179));
						array3[-(~(~(-(~(-(~(~739947880 ^ -1639216200))))))) ^ 157626549 ^ -1154216351] = (byte)((int)array3[~(~(-(~(--832955484 ^ -1872492672))) ^ 97368714 ^ 365926134) ^ -1312567366] ^ ~(--1827095085 ^ -1862383242 ^ 65467597));
						array3[-(-(~(-(~(-(-(~1837633766 ^ -1952879072) ^ -526572411) ^ 704785084)))) ^ 618703835 ^ 141482811)] = (byte)((int)array3[~(-(~(~(~(1495381209 ^ -494440772))))) ^ 1146692997] ^ (-(~(93854557 ^ 862609543) ^ 1354069218) ^ 1715933046));
						num = (-(-(-(-1890516545 ^ 1230530650))) ^ -1406122015 ^ -1782132348);
						continue;
					}
					case 6:
					{
						BindingFlags bindingFlags;
						method8 = typeof(MemberInfo).GetMethod(array[~(~(~(-(-(~(-(~877672116))) ^ -598809853))) ^ 346128870 ^ 2124491822) ^ -2111932290], bindingFlags, null, new Type[-(--753528047 ^ 2082283813) ^ -1957910725 ^ 608670989], null);
						for (;;)
						{
							int num3 = ~(~(-(~(1644653670 ^ 1020831030))) ^ -1591724380);
							for (;;)
							{
								num3 ^= -(-(~(~55)));
								switch (num3 + (~(~(~(~(-(-(1799441473 ^ -411328706 ^ -1374744529 ^ 2086604827 ^ 1369857981)) ^ -520803507)) ^ -604990547)) ^ 885601367))
								{
								case 0:
								{
									Type typeFromHandle3 = typeof(Type);
									string name4 = array[~(~(~(~950881447) ^ 950881442))];
									BindingFlags bindingFlags2;
									BindingFlags bindingAttr4 = bindingFlags2;
									Binder binder4 = null;
									Type[] array6 = new Type[-(-(-(~(-(-(~(~(-2004933867 ^ -258090820))))) ^ -175765341)) ^ 2033842926 ^ -195229212)];
									array6[-(-(-948078811)) ^ -948078811] = typeof(RuntimeTypeHandle);
									method9 = typeFromHandle3.GetMethod(name4, bindingAttr4, binder4, array6, null);
									num3 = (~(-(~(~(-(~-1121787127))))) ^ 1121787135);
									continue;
								}
								case 1:
									num = (~(~(-(-1434893684 ^ 1660624138 ^ 30982169)) ^ 1568315663) ^ -1014812100 ^ -1470313642);
									num3 = ~(~(-(~(-(~28091458) ^ -1354842591)) ^ 1366156695));
									continue;
								case 2:
									method10 = typeof(MemberInfo).GetMethod(array[~(~(-(-(-(-(~-1588061933 ^ 265426477 ^ 1323725852))))) ^ -356366776) ^ -179191149], bindingFlags, null, new Type[-(-(-(-1921324918 ^ -1342436128) ^ -1359916701)) ^ 1938311925], null);
									num3 = (-(-(-(~(~(~(~1987098873))))) ^ 1598606689) ^ 691565983);
									continue;
								case 3:
									method11 = typeof(Type).GetMethod(array[~(~(--276082076) ^ -831103883 ^ 189003057 ^ -986096386 ^ 276322338)], bindingFlags, null, new Type[~(~(~(~(-940021510 ^ -497707386 ^ 632159868))))], null);
									num3 = -(~(~(~(-(-(~(~(--912570928))) ^ -502562928 ^ 483355509) ^ -68096307))) ^ -860897407);
									continue;
								case 4:
									goto IL_DCE;
								}
								break;
							}
						}
						IL_DCE:
						continue;
					}
					case 7:
					{
						byte[] array3;
						array3[~(-(~(-(-(-(-1280842651) ^ -1958582925 ^ 2040416891) ^ -1030102718)))) ^ 1767015028 ^ -357521304] = (byte)((int)array3[~(~(~(-50)))] ^ -(~(-738424360 ^ -738424334)));
						for (;;)
						{
							int num4 = ~(-(~(~(~1394567967 ^ 1195815005)))) ^ 1931204419 ^ -1732381818;
							for (;;)
							{
								switch ((num4 ^ -(-(~(~(~(~(~-717378365) ^ -1566160874)) ^ -1600736443 ^ 1348349296 ^ 2024572197)))) - (~(-(-(-(-(~(-(~-1294763179)) ^ 1178100202))))) ^ -185918724))
								{
								case 0:
								{
									string @string = Encoding.UTF8.GetString(array3);
									char[] array7 = new char[-(-(-(~(-(~(-(~(--905744133)) ^ -925965358 ^ 762053061))))) ^ -799118062)];
									array7[~(~(-1981262705)) ^ -1981262705] = (char)(~(char)(-(char)1511943538 ^ 1095688451 ^ 1662858172) ^ 2018361333);
									array = @string.Split(array7);
									num4 = ~(-(-(~(~(~382880073) ^ 1581874997) ^ 1218131463)));
									continue;
								}
								case 1:
								{
									string name5 = "?";
									MethodAttributes attributes = methodAttributes;
									Type typeFromHandle4 = typeof(string);
									Type[] array8 = new Type[~(~(~(5677691 ^ 903049605 ^ -897963008)))];
									array8[~(-(-(1068299567 ^ -1977760553 ^ 1246678535)))] = typeof(Stream);
									methodBuilder = \uE000.DefineMethod(name5, attributes, typeFromHandle4, array8);
									num4 = -(~(-(-(-(~(~(-(~(-(-(--1319249929))))))))) ^ -1319250047));
									continue;
								}
								case 2:
									num = -(~(-(~(-(~197725544)))) ^ 197725445);
									num4 = ~(~(-(2016044303 ^ 784162077 ^ -1352890983) ^ -1959830046 ^ -1927602199));
									continue;
								case 3:
									array3[-(~(-(~(-247957361 ^ -268690661 ^ 1964530388)) ^ -1081961358 ^ 1918653362 ^ -1567028896 ^ 77330396))] = (byte)((int)array3[~(~(-(-(-(~-1575356268))) ^ -1743651023 ^ -634641678 ^ -534502040))] ^ ~(~(1592077419 ^ 1126432727 ^ -1648890349 ^ -1931869076 ^ -279874014 ^ -470134905)));
									num4 = -(~(-(~(~(~(~(~(-(~1953631226 ^ 743295513 ^ 1480573332)))))))));
									continue;
								case 4:
									goto IL_1058;
								}
								break;
							}
						}
						IL_1058:
						continue;
					}
					case 8:
					{
						byte[] array3;
						array3[~(~(-(~(~(~(~(-(-(-1974812560))) ^ -1955705943 ^ -738515218 ^ 757093100))))))] = (byte)((int)array3[-(-(-(-802428797 ^ 802428760)))] ^ (~(-(-(-(~1146671713)) ^ 1202842684)) ^ -65617521));
						array3[~(-(-(-(-(~(~(~(-(-(~1631139302)))))))) ^ -1631139267))] = (byte)((int)array3[-(-(~(-(~(~(-(~(-(--16995593)) ^ -778915299))) ^ 795744460))))] ^ ~(~(~(-(-(~(1935817360 ^ -1271826982 ^ 1018052293 ^ -1720053675 ^ -117364314)) ^ -611806506) ^ -742815776) ^ -1816699579)));
						array3[~(-(~(-834296662 ^ -1945489518 ^ 1576978433) ^ -531710738))] = (byte)((int)array3[~(-(~(~(395068172 ^ -1091743156 ^ 293930605 ^ -1311861857) ^ 1979450990) ^ 1686783797)) ^ 408733135] ^ ~(~(~(-(-(-(~2013419689)))) ^ 2030244701) ^ -16862120));
						array3[~(~(~(-(-174789652 ^ -182030141) ^ 11705094)))] = (byte)((int)array3[-(-(-(-(~(~(409078965 ^ -1971305754)))) ^ 1887475000 ^ 299169716) ^ -206164743)] ^ -(-(~(-(~(~(~(-(-(-1094568851))))) ^ 1080038058))) ^ 22927706));
						num = (~(-(~(-(~(~(~(~(-(~(-1846470037)))))))) ^ 1969905590)) ^ -459586082);
						continue;
					}
					case 9:
					{
						Type typeFromHandle5 = typeof(BinaryReader);
						BindingFlags bindingFlags;
						BindingFlags bindingAttr5 = bindingFlags;
						Binder binder5 = null;
						Type[] array9 = new Type[-(-(-(-(--1868455609)))) ^ 1366615304 ^ -406142551 ^ -639600103];
						array9[~(~(-(-(-1511719674))) ^ -1511719674)] = typeof(Stream);
						constructor = typeFromHandle5.GetConstructor(bindingAttr5, binder5, array9, null);
						for (;;)
						{
							int num5 = ~(-(~(--6)));
							for (;;)
							{
								num5 ^= ~(~(~(~(-(-(--62))))));
								switch (num5 + ~(-(~157270818) ^ 1228700028 ^ 2038235502 ^ -958315278))
								{
								case 0:
									num = ~(~(-(-(~(~(~(-(-(-924564881))))) ^ -789365784) ^ 404191624)));
									num5 = -(~(~(-(~(~(-(-(-(-67404100))))) ^ -1986362034) ^ 1919024124)));
									continue;
								case 1:
									method12 = typeof(BinaryReader).GetMethod(array[~(~(~(--15602613) ^ -1260707574)) ^ 1271582027], bindingFlags, null, new Type[-(~(--270679178 ^ -190454434) ^ -1457812537 ^ -1227253501 ^ 79312111)], null);
									num5 = (~(~(-(~(~(~(-(~(-726624040 ^ 254218473 ^ 1419136989 ^ 670154403 ^ -1742960523)))))))) ^ -820834619);
									continue;
								case 2:
									constructor2 = typeof(Hashtable).GetConstructor(bindingFlags, null, new Type[-(~(~(-(-(-(-15508616 ^ -1902333491) ^ -1905257141)))))], null);
									num5 = (~(-(-(~(-(~(-680444383))) ^ 572851675)) ^ -1947813231) ^ -2125627757);
									continue;
								case 3:
								{
									Type typeFromHandle6 = typeof(Hashtable);
									string name6 = array[~(-(~(-(-614980113)) ^ -614980125))];
									BindingFlags bindingAttr6 = bindingFlags;
									Binder binder6 = null;
									Type[] array10 = new Type[-(-(-(-(~(-(-(~(1163438379 ^ 2050414012)))) ^ -218538747 ^ -1994141476))) ^ 1152708940)];
									array10[~(-(-(~(-(2145265227 ^ 947793485 ^ 1201670150)))))] = typeof(object);
									array10[~(-(~(-(--1758551909)) ^ -754393898 ^ -517053555)) ^ 1526194239] = typeof(object);
									method13 = typeFromHandle6.GetMethod(name6, bindingAttr6, binder6, array10, null);
									num5 = ~(-(-(~(~(~1152639818 ^ 760151971 ^ -867392784))) ^ 1515078113));
									continue;
								}
								case 4:
									goto IL_146E;
								}
								break;
							}
						}
						IL_146E:
						continue;
					}
					case 10:
					{
						BindingFlags bindingFlags = -(~(-(~(BindingFlags)(-959134457)) ^ (BindingFlags)(-959134405)));
						BindingFlags bindingFlags2 = -(-(-(-(-(BindingFlags)1872905520) ^ (BindingFlags)1011715557)) ^ (BindingFlags)(-1408223459));
						byte[] array3 = Convert.FromBase64String("WT/qY3IKPAxJDEszFwgCAUk5Pg9zSXSDJVQ0CD1rbhzKfG9dBhVoNRlcMzcLTRSvQE47R2V0VHlwSEZyb20uYW5kbGU7Z2V0X05hbWU7SW5kZXhPZjtFeGl0O2dldF9GcmFtZUNvdW50O2dldF9MZW5ndGg7UmVhZFN0cmluZztBZGQ7Z2V0X1Bvc2l0aW9uO2dldF9DdXJyZW50RG9tYWluO1NldERhdGE7UnVudGltZU1ldGhvZDtTeXN0ZW0uRGlhZ25vc3RpY3MuU3RhY2tUcmFjZTtTeXN0ZW0uRGlhZ25vc3RpY3MuU3RhY2tGcmFtZTsxMzYwNTtTeXN0ZW0uRW52aXJvbm1lbnQ=");
						array3[-(~(-(-(-171863832))) ^ 171863831)] = (byte)((int)array3[-(-(~(~(~(~(~1823384971))) ^ -736278456 ^ -919506674))) ^ -1904385230] ^ -(~(-(-(-(-(~(~(~(-(~(1308162809 ^ 952105895))))))))) ^ -1967587651)));
						num = (-(-(--175853740) ^ -1646040457) ^ -1751631198);
						continue;
					}
					case 11:
					{
						byte[] array3;
						array3[~(~(~(~(-(-(-(~849143040)))) ^ -710133837 ^ 1447710596))) ^ -1317364421] = (byte)((int)array3[-(~(-(~(-(~(~(715253874 ^ -62382760)))))) ^ 1956697208 ^ 1198683212 ^ -998892082) ^ -558713053] ^ -(-(-(-(-(~(1815699169 ^ 1243565586 ^ -1897352898 ^ -1466076753 ^ -458029675)) ^ -1654593293))) ^ 2038447462));
						array3[~(-(~(~(-(~(~(~(-(-705358204 ^ 1500953383)))) ^ 1445204159))) ^ 1802884395 ^ 1311610823))] = (byte)((int)array3[-(~(~(~(625510105 ^ 625510100))))] ^ -(~(-(-(~(-1459199641)) ^ 1459199715))));
						array3[~(~(~(-(~(--1390489084)) ^ -1390489075)))] = (byte)((int)array3[-(-(~(-(-(~1527806944)) ^ 816184799)) ^ 1807058480)] ^ -(-(-(-2012464758 ^ -842279737) ^ -536048564 ^ 1513397910)));
						array3[~(~(-(-(--1210094229)) ^ 1210094213))] = (byte)((int)array3[~(~(~(-1998467333 ^ 756877534) ^ 1510160842))] ^ (~(-(-690335617 ^ -1724827184 ^ -964778748) ^ 2099239475 ^ 771370591) ^ -646956832));
						num = ~(~(-1274972895 ^ 506214063 ^ 132090375 ^ -887601453 ^ 260918391 ^ -1419761207 ^ -1039668587));
						continue;
					}
					case 12:
					{
						byte[] array3;
						array3[-(~(~(-(-(-(~(-(~(~860077004)) ^ 1624131688) ^ 984634257) ^ 1765646389)))))] = (byte)((int)array3[-(-(~(-(~(-(-566932515 ^ -2144882740 ^ -412801707)) ^ 1218613117)) ^ -1107532156)) ^ 1277765820] ^ -(-(-(-(-(~(~(~-233259988) ^ 502508751)) ^ -269807473)))));
						array3[~(-296949961 ^ -1153297867) ^ -1539842432 ^ -1913504789 ^ -1493686506 ^ 634177670] = (byte)((int)array3[~(-1959512407 ^ -2011508131 ^ 1574217926 ^ -1593461813)] ^ (~(~(~(~-388192458 ^ 880736425) ^ -1240840157)) ^ 1789401069));
						array3[-(-(~(-513787000))) ^ 513786992] = (byte)((int)array3[-(~(~(~(-(-(943056540 ^ 2144205088) ^ 1207443390)))))] ^ ~(~(-(~(~-105)))));
						array3[-(-(-(-(~772093413 ^ 1060430933))) ^ -288477623)] = (array3[~(~(-(~(~(~(~(~(~1848787369 ^ -1908984643) ^ -79143352)) ^ 2090359645 ^ -1742041095)))))] ^ -(~(-(~112))));
						num = ~(~(-(-1664457428 ^ -1917846320)) ^ -1646043288 ^ 1937324318);
						continue;
					}
					case 13:
					{
						byte[] array3;
						array3[~(~(-(-(~(~(~(-(--2))))))))] = (byte)((int)array3[~(~(-(~(~(-875579769)) ^ -1085576925))) ^ -1954865059] ^ (-(~(-(~(-(~(~(1871839297 ^ -918214639 ^ 1508778471))))) ^ 1622903647)) ^ 534211808 ^ 2141852076));
						array3[-(~(~(~(~(-(~(~(-(1170397387 ^ 123459033) ^ -804469732) ^ 264238933))))) ^ 1658175911))] = (byte)((int)array3[~(-(-(-(-(-988047800) ^ 988047803))))] ^ ~(-(-(-(--1071008438) ^ -451920135) ^ -624528174)));
						array3[~(-(-(-(-1888151388 ^ 983047704 ^ -1627553240) ^ 1669330346)) ^ 1215263034)] = (byte)((int)array3[-(~(~(-(-(-1400847702)) ^ 1994072757) ^ 631513058))] ^ (~(-(~(1906540645 ^ -1592466413 ^ -2068555450 ^ -278974156 ^ 1008971410)) ^ 898701157) ^ 1034138155 ^ 1890777091));
						array3[-(~(~(-(~-5))))] = (byte)((int)array3[-(-(-(~-1690678267)) ^ 966157380) ^ -1565775802] ^ (~(~(-(--945976523 ^ 2067443054))) ^ 2068710612 ^ -1287318258 ^ 1957443457));
						num = -(-(-(~(-(~(~-523811973 ^ 450440418)) ^ 2028144288)) ^ 2097321652));
						continue;
					}
					case 14:
					{
						byte[] array3;
						array3[-(~(~(-188926885 ^ -884759454 ^ 93125100)) ^ -980576714)] = (byte)((int)array3[-(-(-(~(~(-(~(~(-(618373596 ^ -1150761801)) ^ -888603834)) ^ 1259833782)))) ^ -531371912)] ^ -(~(-(-(-(-(-(--189913995)) ^ -1940701468)) ^ -1404545234)) ^ -726315586));
						array3[-(-(-(~(-(-(~-949841780)) ^ -1102749044) ^ -1363569254)) ^ 677484637)] = (byte)((int)array3[~(-(-(~(-(~(-(-56)))))))] ^ -(-(--1901472260) ^ 1901472303));
						array3[-(~(~(-941829483) ^ 941829492))] = (byte)((int)array3[-(-(-(-(-(-1694741959))) ^ 367293986) ^ 2049599988) ^ -1487859192 ^ 24390287 ^ -1394044279] ^ ~(~(~(~(-(~(-(-(~(~1482503745))))) ^ 1482503737)))));
						array3[-(-(-(~(-(~(~(-(207381301 ^ 1335795817))) ^ 1136837445)))))] = (byte)((int)array3[~(-(-(-2126482392 ^ 610971343 ^ -1217650319))) ^ -306641847] ^ (-(-(-(-(808162821 ^ -1128496232 ^ 146707919) ^ -3328496))) ^ -281674201 ^ -264224336 ^ 1687226699));
						num = -(-(~(-(-1795517040 ^ 1405354370 ^ -952200170))));
						continue;
					}
					case 15:
					{
						byte[] array3;
						array3[-(~(~-30504710 ^ 1202394863)) ^ 1182425594] = (byte)((int)array3[-(~(~(~(-(~(~(~-1939963997)) ^ 1099458039)) ^ 464263423)) ^ -696646982)] ^ (~(~296158355 ^ -1215756777) ^ -1506934055));
						array3[-(~1514104669 ^ -609498408 ^ 917823965) ^ -1222620085] = (byte)((int)array3[~(-1768909325 ^ -429443309) ^ -82967557 ^ 1679835165 ^ 270933739] ^ ~(~(-(-(-(~(-(-(--901515775) ^ 894324113))) ^ -1389126990) ^ -1379803450))));
						array3[-(~(~(~(~(~(-(-(--14049388)) ^ 1520045564)))))) ^ -333951929 ^ -1236000315] = (byte)((int)array3[-(-(-(~(-855631936) ^ -2096651795)) ^ -1362810302 ^ -94576357) ^ 446594406] ^ (-(~(-(~(~(-196680403 ^ 1566778986 ^ 1708204096))) ^ -1091296506)) ^ -1912651672));
						array3[-(~(-(-(~(~(~1217829099)) ^ 1340096768)) ^ 1457637231 ^ -1368858264))] = (byte)((int)array3[~(-(~(-(286150840 ^ 569974445) ^ 1587806372 ^ 1850953381)))] ^ (~(-(~(~172832144)) ^ 1256062132) ^ 1083230509));
						num = (-(-(1173978865 ^ -1375912665)) ^ -943769445 ^ 800815934);
						continue;
					}
					case 16:
					{
						Type type = Type.GetType(array[-(-1726326597 ^ -314299295 ^ -1736017528) ^ 321385151]);
						BindingFlags bindingFlags;
						constructor3 = type.GetConstructor(bindingFlags, null, new Type[-(~(-(-(-(-(-(~(-(~1838736029)))) ^ 1658295630))))) ^ 256866258], null);
						Type type3 = type;
						string name7 = array[~(-(-(~(~(~(~(~(~(-1781370459 ^ -1931476629))) ^ -422736079))))))];
						BindingFlags bindingAttr7 = bindingFlags;
						Binder binder7 = null;
						Type[] array11 = new Type[~(-(~(-(-(~(~(-(~-102208991))) ^ 102208991)))))];
						array11[-(~(-(-(~(~(--648097794 ^ 723552388 ^ 1318871821 ^ 1604505118 ^ 681094723)) ^ 460605905)) ^ -794030600))] = typeof(int);
						method14 = type3.GetMethod(name7, bindingAttr7, binder7, array11, null);
						method15 = Type.GetType(array[-(-(-(1367925130 ^ -652623125 ^ 1642853961) ^ 186572360) ^ 382195851) ^ 190589975]).GetMethod(array[~(-(-(~(-(~(-(~-1787810740 ^ 631803500))))) ^ -1022254233 ^ 1313554004)) ^ 1031945998], bindingFlags, null, new Type[~(-(~(~(~(~1437945108))) ^ 514698778)) ^ 1259922189], null);
						num = ~(-(~(~(-(~(-(~(-1411976221) ^ -1411976218)))))));
						continue;
					}
					case 17:
					{
						byte[] array3;
						array3[-(~(-(~812531814))) ^ 812531837] = (byte)((int)array3[~(-(-(-95844692 ^ 652124728 ^ 1360516154 ^ 2139728363))) ^ -2141190831 ^ -1919507458] ^ -(-(~(~(~(-(~1694834645) ^ 599691690)) ^ -1717546315)) ^ 551822089));
						array3[-(~(-(~(-1024543544 ^ -1024543524))))] = (byte)((int)array3[~(-(~(-(~(~(-(-(~876126233) ^ -1068791422 ^ 1337139843))) ^ 1145042173))))] ^ ~(~(-(-(~(-(-(1985145862 ^ 2135052716)) ^ -1390126530)))) ^ 1540036672));
						array3[~(~(-(967491446 ^ 154115791 ^ -1049286256) ^ 235927488))] = (byte)((int)array3[-(-(~(-1424996166 ^ 735561796 ^ 2134408982)))] ^ ~(-(-(~(8414246 ^ 121535564))) ^ 129898669));
						array3[~(~(~(-2047022145 ^ 937594692)) ^ 1306630940)] = (byte)((int)array3[-(~(~(-(--885213937 ^ 885213929))))] ^ (-(-(~(-(-(-(-(-(-1706766761 ^ -1945450753) ^ 601239401 ^ -1518756845))))) ^ -635073124)) ^ 1254372865));
						num = ~(-(-(-1140544190)) ^ 1140544186);
						continue;
					}
					case 18:
					{
						byte[] array3;
						array3[~(-997457590 ^ -46866781 ^ 733917829) ^ -969658554 ^ 734903772] = (byte)((int)array3[-(-(-(~(-1917985540 ^ -667285674))) ^ 1436015522)] ^ -(~(~(~(~(~(-224011618 ^ -1293188328 ^ 2068207844)) ^ -2076959156))) ^ -1086578844));
						array3[~(-(-(~(-(~(~(-(~764457849) ^ 1001009691)))))) ^ -1968803981 ^ 2064953996) ^ 410493802] = (byte)((int)array3[-(-(~(-11)))] ^ (-(-(~(-(-(~(~(-658589871 ^ -113501350)) ^ 1357461714)) ^ -377883878)) ^ 1938339260) ^ -1895548963 ^ -1688135045));
						array3[-(~(~(--1206441586) ^ 1363183802 ^ -380133059))] = (byte)((int)array3[~(~(~(-(1201622143 ^ 356392225) ^ 461649768 ^ 1712919142 ^ 1860815326 ^ -1653197902))) ^ -593261004] ^ (~(~(-(~(-(-824616522 ^ -607787097) ^ -1246478339)))) ^ -1468580625 ^ -1103099371 ^ 1231046830));
						array3[-(~(-(--87235841) ^ 901534033 ^ -814697563))] = (byte)((int)array3[~(~(-(-(-(~(~-1349368979)))) ^ -1025493320 ^ -1836217817))] ^ (-(-(-(~(--1736889268 ^ -869577176) ^ 193518231 ^ -1106077313))) ^ 506977839));
						num = ~(~(~(-(-1538577138) ^ -1979706634)) ^ 1557744168 ^ 1922201006);
						continue;
					}
					case 19:
						goto IL_2109;
					}
					break;
				}
			}
			IL_2109:
			methodBuilder.DefineParameter(-(~(-1111257827 ^ 2002130765 ^ 644536680)) ^ -511516788 ^ 226335924, -(-(~(~(~(-(-(ParameterAttributes)2024234131))) ^ (ParameterAttributes)(-218997236) ^ (ParameterAttributes)876742732) ^ (ParameterAttributes)1427469703) ^ (ParameterAttributes)(-352157867)), "a");
			ILGenerator ilgenerator = methodBuilder.GetILGenerator();
			ilgenerator.DeclareLocal(typeof(StackTrace));
			ilgenerator.DeclareLocal(typeof(int));
			ilgenerator.DeclareLocal(typeof(StackFrame));
			ilgenerator.DeclareLocal(typeof(Type));
			ilgenerator.DeclareLocal(typeof(string));
			ilgenerator.DeclareLocal(typeof(long));
			ilgenerator.DeclareLocal(typeof(BinaryReader));
			ilgenerator.DeclareLocal(typeof(Hashtable));
			ilgenerator.DeclareLocal(typeof(string));
			Label label = ilgenerator.DefineLabel();
			Label label2 = ilgenerator.DefineLabel();
			Label label3 = ilgenerator.DefineLabel();
			Label label4 = ilgenerator.DefineLabel();
			Label label5 = ilgenerator.DefineLabel();
			Label label6 = ilgenerator.DefineLabel();
			ilgenerator.Emit(OpCodes.Newobj, constructor3);
			ilgenerator.Emit(OpCodes.Stloc_0);
			ilgenerator.Emit(OpCodes.Ldc_I4_8);
			ilgenerator.Emit(OpCodes.Stloc_1);
			ilgenerator.Emit(OpCodes.Br_S, label);
			ilgenerator.MarkLabel(label4);
			ilgenerator.Emit(OpCodes.Ldloc_0);
			ilgenerator.Emit(OpCodes.Ldloc_1);
			ilgenerator.Emit(OpCodes.Callvirt, method14);
			ilgenerator.Emit(OpCodes.Stloc_2);
			ilgenerator.Emit(OpCodes.Ldloc_2);
			ilgenerator.Emit(OpCodes.Callvirt, method15);
			ilgenerator.Emit(OpCodes.Callvirt, method8);
			ilgenerator.Emit(OpCodes.Stloc_3);
			ilgenerator.Emit(OpCodes.Ldloc_3);
			ilgenerator.Emit(OpCodes.Brfalse_S, label2);
			ilgenerator.Emit(OpCodes.Ldloc_3);
			ilgenerator.Emit(OpCodes.Callvirt, method11);
			ilgenerator.Emit(OpCodes.Stloc_S, ~(-(~(-(-(~(~(-867830893 ^ 2105682437)))))) ^ 1312302700));
			ilgenerator.Emit(OpCodes.Ldloc_S, ~(~(-(-(~(-(-204636981 ^ -1398016446))) ^ -1782257702)) ^ -975140337) ^ 256042837);
			ilgenerator.Emit(OpCodes.Ldtoken, typeof(\uE002));
			ilgenerator.Emit(OpCodes.Call, method9);
			ilgenerator.Emit(OpCodes.Callvirt, method10);
			ilgenerator.Emit(OpCodes.Callvirt, method4);
			ilgenerator.Emit(OpCodes.Ldc_I4_M1);
			ilgenerator.Emit(OpCodes.Bne_Un_S, label3);
			ilgenerator.Emit(OpCodes.Ldloc_S, -(-(~(-(~(-(~(~(~-1043569766)))) ^ -2037349973) ^ -1237643769))) ^ 244847565);
			ilgenerator.Emit(OpCodes.Ldstr, array[-(~(~(-(~-17))))]);
			ilgenerator.Emit(OpCodes.Callvirt, method4);
			ilgenerator.Emit(OpCodes.Ldc_I4_M1);
			ilgenerator.Emit(OpCodes.Beq_S, label2);
			ilgenerator.Emit(OpCodes.Ldc_I4_0);
			ilgenerator.Emit(OpCodes.Call, method6);
			ilgenerator.MarkLabel(label3);
			ilgenerator.Emit(OpCodes.Ldloc_1);
			ilgenerator.Emit(OpCodes.Ldc_I4_1);
			ilgenerator.Emit(OpCodes.Add);
			ilgenerator.Emit(OpCodes.Stloc_1);
			ilgenerator.MarkLabel(label);
			ilgenerator.Emit(OpCodes.Ldloc_1);
			ilgenerator.Emit(OpCodes.Ldloc_0);
			ilgenerator.Emit(OpCodes.Callvirt, method5);
			ilgenerator.Emit(OpCodes.Blt_S, label4);
			ilgenerator.MarkLabel(label2);
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Callvirt, method7);
			ilgenerator.Emit(OpCodes.Stloc_S, -(~(-(~(-(-(-(~(~(-502193931 ^ 1183969893))))) ^ 1535061363)))));
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Newobj, constructor);
			ilgenerator.Emit(OpCodes.Stloc_S, -(--1948686618 ^ 863713757) ^ -1197318337);
			ilgenerator.Emit(OpCodes.Newobj, constructor2);
			ilgenerator.Emit(OpCodes.Stloc_S, ~(-(~(-1765657032 ^ -1559252857))) ^ -902661832);
			ilgenerator.Emit(OpCodes.Ldloc_S, ~(-(~1369172515)) ^ -651828392 ^ 725231789 ^ 1551621672);
			ilgenerator.Emit(OpCodes.Callvirt, method12);
			ilgenerator.Emit(OpCodes.Stloc_S, -(-(-(-(~(~(~(~(-200053192))) ^ 1910739504)) ^ -1099495516 ^ 1404954243) ^ 1748639017)));
			ilgenerator.Emit(OpCodes.Ldloc_S, ~(~(-(--1703325450)) ^ -1703325455));
			ilgenerator.Emit(OpCodes.Ldc_I4_M1);
			ilgenerator.Emit(OpCodes.Box, typeof(int));
			ilgenerator.Emit(OpCodes.Ldloc_S, ~(-(-(~(-(-(~(~2105030602 ^ 2105030594))))))));
			ilgenerator.Emit(OpCodes.Callvirt, method13);
			ilgenerator.Emit(OpCodes.Br_S, label5);
			ilgenerator.MarkLabel(label6);
			ilgenerator.Emit(OpCodes.Ldloc_S, -(-(~(-(~(-(~46658412))) ^ -888572534)) ^ 909211932));
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Callvirt, method);
			ilgenerator.Emit(OpCodes.Conv_I4);
			ilgenerator.Emit(OpCodes.Ldc_I4, ~(-290893176) ^ 2009665375 ^ 1721737855);
			ilgenerator.Emit(OpCodes.Add);
			int arg = int.Parse(array[~(~(~(-(~(-(-222081232 ^ -1638893936))))) ^ 1821587853)]);
			ilgenerator.Emit(OpCodes.Ldc_I4, arg);
			ilgenerator.Emit(OpCodes.Xor);
			ilgenerator.Emit(OpCodes.Box, typeof(int));
			ilgenerator.Emit(OpCodes.Ldloc_S, ~(-(~(-(~1085862029 ^ -1394765182 ^ -399013271)) ^ 1819915877 ^ -1747672582)));
			ilgenerator.Emit(OpCodes.Callvirt, method12);
			ilgenerator.Emit(OpCodes.Callvirt, method13);
			ilgenerator.MarkLabel(label5);
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Callvirt, method);
			ilgenerator.Emit(OpCodes.Ldloc_S, -(-(-(~1790669705 ^ 1790669709))));
			ilgenerator.Emit(OpCodes.Blt_S, label6);
			ilgenerator.Emit(OpCodes.Call, method2);
			ilgenerator.Emit(OpCodes.Ldloc_S, -(-(-(-1381407721 ^ -2010368259) ^ 150416246) ^ -762476968));
			ilgenerator.Emit(OpCodes.Ldloc_S, -(-(~(~(-(~1139120347 ^ 329663153 ^ 944873302) ^ -566464063 ^ 1548335839)) ^ 1990366388) ^ -1664804208));
			ilgenerator.Emit(OpCodes.Callvirt, method3);
			ilgenerator.Emit(OpCodes.Ldloc_S, ~(~(-(-(-(~(~(-(-264514368))))))) ^ 302203235 ^ -320217162) ^ 248597021);
			ilgenerator.Emit(OpCodes.Ret);
			return methodBuilder;
		}

		public string \uE001(Stream \uE000)
		{
			TypeBuilder typeBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName
			{
				Name = "?"
			}, ~(~(-(-(-(-(-((AssemblyBuilderAccess)(-229031838) ^ (AssemblyBuilderAccess)1229637335) ^ (AssemblyBuilderAccess)713796445) ^ (AssemblyBuilderAccess)243166829) ^ (AssemblyBuilderAccess)1612299384))))).DefineDynamicModule("?").DefineType("?", ~(-(~(-(~(~(~(TypeAttributes)(-1254135162) ^ (TypeAttributes)739024947)))) ^ (TypeAttributes)687531348 ^ (TypeAttributes)1312215068)));
			this.\uE000(typeBuilder);
			Type type = typeBuilder.CreateType();
			string name = "?";
			BindingFlags invokeAttr = -(~(~(-(~(~(-(BindingFlags)(-1392324135)) ^ (BindingFlags)2143994317) ^ (BindingFlags)(-604812598) ^ (BindingFlags)(-1355528182) ^ (BindingFlags)1508961330))));
			Binder binder = null;
			object target = null;
			object[] array = new object[~(~(~(~(~(~(~(-(860881575 ^ -329189759))) ^ 597594079)))) ^ -55673861)];
			array[~(~(~(~(-1762545732 ^ -1482718170 ^ 791882603 ^ -1267574239)) ^ -1439737136))] = \uE000;
			return (string)type.InvokeMember(name, invokeAttr, binder, target, array);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static \uE009()
		{
			for (;;)
			{
				int num = ~(~(-(~(~(-(~(~1131006879)) ^ 914039683) ^ -1921118434) ^ 996933068) ^ 1350884244) ^ -1820081859);
				for (;;)
				{
					switch ((num ^ ~(-(~(~(-(-57)))))) - ~(-(~(1949509867 ^ -748053276 ^ 1487227850))))
					{
					case 0:
						num = -(-(-(-(-(1570264859 ^ 503614831 ^ 916881871))) ^ -2113020574 ^ 147355997));
						continue;
					case 1:
						num = (-(~(-(--907535521 ^ 172517196 ^ -1981497239) ^ 1949329174)) ^ 1047845738);
						continue;
					case 2:
						num = -(-(~(-(~(~(~(-(~(~(-(--953103738)))))) ^ -953103741)))));
						continue;
					case 3:
						num = ~(~(-(72178403 ^ -72178407)));
						continue;
					case 4:
						num = -(-(~(-(-(-201747225 ^ 201747231)))));
						continue;
					case 5:
						num = -(-(~(-(~(~(~(~121)))))));
						continue;
					case 6:
						num = -(-(-(1563933576 ^ -1571072387 ^ 9638411)));
						continue;
					case 7:
						num = ~(-(~(~(~-510170436 ^ -1601590471) ^ -1092534658)));
						continue;
					case 8:
						num = ~(~(~(35150749 ^ -1409816989)) ^ 1443906053);
						continue;
					case 9:
						return;
					}
					break;
				}
			}
		}
	}
}
