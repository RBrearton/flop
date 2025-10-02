namespace Flop.Core;

/// <summary>
/// Represents an RGBA color with 8-bit channels.
/// </summary>
public readonly record struct Color(byte R, byte G, byte B, byte A = 255)
{
    // Basic colors
    public static readonly Color White = new(255, 255, 255);
    public static readonly Color Black = new(0, 0, 0);
    public static readonly Color Transparent = new(0, 0, 0, 0);

    // Material Design Color System
    // https://material.io/design/color/the-color-system.html

    // Red
    public static readonly Color Red_50 = new(255, 235, 238);
    public static readonly Color Red_100 = new(255, 205, 210);
    public static readonly Color Red_200 = new(239, 154, 154);
    public static readonly Color Red_300 = new(229, 115, 115);
    public static readonly Color Red_400 = new(239, 83, 80);
    public static readonly Color Red_500 = new(244, 67, 54);
    public static readonly Color Red_600 = new(229, 57, 53);
    public static readonly Color Red_700 = new(211, 47, 47);
    public static readonly Color Red_800 = new(198, 40, 40);
    public static readonly Color Red_900 = new(183, 28, 28);
    public static readonly Color Red_A100 = new(255, 138, 128);
    public static readonly Color Red_A200 = new(255, 82, 82);
    public static readonly Color Red_A400 = new(255, 23, 68);
    public static readonly Color Red_A700 = new(213, 0, 0);

    // Pink
    public static readonly Color Pink_50 = new(252, 228, 236);
    public static readonly Color Pink_100 = new(248, 187, 208);
    public static readonly Color Pink_200 = new(244, 143, 177);
    public static readonly Color Pink_300 = new(240, 98, 146);
    public static readonly Color Pink_400 = new(236, 64, 122);
    public static readonly Color Pink_500 = new(233, 30, 99);
    public static readonly Color Pink_600 = new(216, 27, 96);
    public static readonly Color Pink_700 = new(194, 24, 91);
    public static readonly Color Pink_800 = new(173, 20, 87);
    public static readonly Color Pink_900 = new(136, 14, 79);
    public static readonly Color Pink_A100 = new(255, 128, 171);
    public static readonly Color Pink_A200 = new(255, 64, 129);
    public static readonly Color Pink_A400 = new(245, 0, 87);
    public static readonly Color Pink_A700 = new(197, 17, 98);

    // Purple
    public static readonly Color Purple_50 = new(243, 229, 245);
    public static readonly Color Purple_100 = new(225, 190, 231);
    public static readonly Color Purple_200 = new(206, 147, 216);
    public static readonly Color Purple_300 = new(186, 104, 200);
    public static readonly Color Purple_400 = new(171, 71, 188);
    public static readonly Color Purple_500 = new(156, 39, 176);
    public static readonly Color Purple_600 = new(142, 36, 170);
    public static readonly Color Purple_700 = new(123, 31, 162);
    public static readonly Color Purple_800 = new(106, 27, 154);
    public static readonly Color Purple_900 = new(74, 20, 140);
    public static readonly Color Purple_A100 = new(234, 128, 252);
    public static readonly Color Purple_A200 = new(224, 64, 251);
    public static readonly Color Purple_A400 = new(213, 0, 249);
    public static readonly Color Purple_A700 = new(170, 0, 255);

    // Deep Purple
    public static readonly Color DeepPurple_50 = new(237, 231, 246);
    public static readonly Color DeepPurple_100 = new(209, 196, 233);
    public static readonly Color DeepPurple_200 = new(179, 157, 219);
    public static readonly Color DeepPurple_300 = new(149, 117, 205);
    public static readonly Color DeepPurple_400 = new(126, 87, 194);
    public static readonly Color DeepPurple_500 = new(103, 58, 183);
    public static readonly Color DeepPurple_600 = new(94, 53, 177);
    public static readonly Color DeepPurple_700 = new(81, 45, 168);
    public static readonly Color DeepPurple_800 = new(69, 39, 160);
    public static readonly Color DeepPurple_900 = new(49, 27, 146);
    public static readonly Color DeepPurple_A100 = new(179, 136, 255);
    public static readonly Color DeepPurple_A200 = new(124, 77, 255);
    public static readonly Color DeepPurple_A400 = new(101, 31, 255);
    public static readonly Color DeepPurple_A700 = new(98, 0, 234);

    // Indigo
    public static readonly Color Indigo_50 = new(232, 234, 246);
    public static readonly Color Indigo_100 = new(197, 202, 233);
    public static readonly Color Indigo_200 = new(159, 168, 218);
    public static readonly Color Indigo_300 = new(121, 134, 203);
    public static readonly Color Indigo_400 = new(92, 107, 192);
    public static readonly Color Indigo_500 = new(63, 81, 181);
    public static readonly Color Indigo_600 = new(57, 73, 171);
    public static readonly Color Indigo_700 = new(48, 63, 159);
    public static readonly Color Indigo_800 = new(40, 53, 147);
    public static readonly Color Indigo_900 = new(26, 35, 126);
    public static readonly Color Indigo_A100 = new(140, 158, 255);
    public static readonly Color Indigo_A200 = new(83, 109, 254);
    public static readonly Color Indigo_A400 = new(61, 90, 254);
    public static readonly Color Indigo_A700 = new(48, 79, 254);

    // Blue
    public static readonly Color Blue_50 = new(227, 242, 253);
    public static readonly Color Blue_100 = new(187, 222, 251);
    public static readonly Color Blue_200 = new(144, 202, 249);
    public static readonly Color Blue_300 = new(100, 181, 246);
    public static readonly Color Blue_400 = new(66, 165, 245);
    public static readonly Color Blue_500 = new(33, 150, 243);
    public static readonly Color Blue_600 = new(30, 136, 229);
    public static readonly Color Blue_700 = new(25, 118, 210);
    public static readonly Color Blue_800 = new(21, 101, 192);
    public static readonly Color Blue_900 = new(13, 71, 161);
    public static readonly Color Blue_A100 = new(130, 177, 255);
    public static readonly Color Blue_A200 = new(68, 138, 255);
    public static readonly Color Blue_A400 = new(41, 121, 255);
    public static readonly Color Blue_A700 = new(41, 98, 255);

    // Light Blue
    public static readonly Color LightBlue_50 = new(225, 245, 254);
    public static readonly Color LightBlue_100 = new(179, 229, 252);
    public static readonly Color LightBlue_200 = new(129, 212, 250);
    public static readonly Color LightBlue_300 = new(79, 195, 247);
    public static readonly Color LightBlue_400 = new(41, 182, 246);
    public static readonly Color LightBlue_500 = new(3, 169, 244);
    public static readonly Color LightBlue_600 = new(3, 155, 229);
    public static readonly Color LightBlue_700 = new(2, 136, 209);
    public static readonly Color LightBlue_800 = new(2, 119, 189);
    public static readonly Color LightBlue_900 = new(1, 87, 155);
    public static readonly Color LightBlue_A100 = new(128, 216, 255);
    public static readonly Color LightBlue_A200 = new(64, 196, 255);
    public static readonly Color LightBlue_A400 = new(0, 176, 255);
    public static readonly Color LightBlue_A700 = new(0, 145, 234);

    // Cyan
    public static readonly Color Cyan_50 = new(224, 247, 250);
    public static readonly Color Cyan_100 = new(178, 235, 242);
    public static readonly Color Cyan_200 = new(128, 222, 234);
    public static readonly Color Cyan_300 = new(77, 208, 225);
    public static readonly Color Cyan_400 = new(38, 198, 218);
    public static readonly Color Cyan_500 = new(0, 188, 212);
    public static readonly Color Cyan_600 = new(0, 172, 193);
    public static readonly Color Cyan_700 = new(0, 151, 167);
    public static readonly Color Cyan_800 = new(0, 131, 143);
    public static readonly Color Cyan_900 = new(0, 96, 100);
    public static readonly Color Cyan_A100 = new(132, 255, 255);
    public static readonly Color Cyan_A200 = new(24, 255, 255);
    public static readonly Color Cyan_A400 = new(0, 229, 255);
    public static readonly Color Cyan_A700 = new(0, 184, 212);

    // Teal
    public static readonly Color Teal_50 = new(224, 242, 241);
    public static readonly Color Teal_100 = new(178, 223, 219);
    public static readonly Color Teal_200 = new(128, 203, 196);
    public static readonly Color Teal_300 = new(77, 182, 172);
    public static readonly Color Teal_400 = new(38, 166, 154);
    public static readonly Color Teal_500 = new(0, 150, 136);
    public static readonly Color Teal_600 = new(0, 137, 123);
    public static readonly Color Teal_700 = new(0, 121, 107);
    public static readonly Color Teal_800 = new(0, 105, 92);
    public static readonly Color Teal_900 = new(0, 77, 64);
    public static readonly Color Teal_A100 = new(167, 255, 235);
    public static readonly Color Teal_A200 = new(100, 255, 218);
    public static readonly Color Teal_A400 = new(29, 233, 182);
    public static readonly Color Teal_A700 = new(0, 191, 165);

    // Green
    public static readonly Color Green_50 = new(232, 245, 233);
    public static readonly Color Green_100 = new(200, 230, 201);
    public static readonly Color Green_200 = new(165, 214, 167);
    public static readonly Color Green_300 = new(129, 199, 132);
    public static readonly Color Green_400 = new(102, 187, 106);
    public static readonly Color Green_500 = new(76, 175, 80);
    public static readonly Color Green_600 = new(67, 160, 71);
    public static readonly Color Green_700 = new(56, 142, 60);
    public static readonly Color Green_800 = new(46, 125, 50);
    public static readonly Color Green_900 = new(27, 94, 32);
    public static readonly Color Green_A100 = new(185, 246, 202);
    public static readonly Color Green_A200 = new(105, 240, 174);
    public static readonly Color Green_A400 = new(0, 230, 118);
    public static readonly Color Green_A700 = new(0, 200, 83);

    // Light Green
    public static readonly Color LightGreen_50 = new(241, 248, 233);
    public static readonly Color LightGreen_100 = new(220, 237, 200);
    public static readonly Color LightGreen_200 = new(197, 225, 165);
    public static readonly Color LightGreen_300 = new(174, 213, 129);
    public static readonly Color LightGreen_400 = new(156, 204, 101);
    public static readonly Color LightGreen_500 = new(139, 195, 74);
    public static readonly Color LightGreen_600 = new(124, 179, 66);
    public static readonly Color LightGreen_700 = new(104, 159, 56);
    public static readonly Color LightGreen_800 = new(85, 139, 47);
    public static readonly Color LightGreen_900 = new(51, 105, 30);
    public static readonly Color LightGreen_A100 = new(204, 255, 144);
    public static readonly Color LightGreen_A200 = new(178, 255, 89);
    public static readonly Color LightGreen_A400 = new(118, 255, 3);
    public static readonly Color LightGreen_A700 = new(100, 221, 23);

    // Lime
    public static readonly Color Lime_50 = new(249, 251, 231);
    public static readonly Color Lime_100 = new(240, 244, 195);
    public static readonly Color Lime_200 = new(230, 238, 156);
    public static readonly Color Lime_300 = new(220, 231, 117);
    public static readonly Color Lime_400 = new(212, 225, 87);
    public static readonly Color Lime_500 = new(205, 220, 57);
    public static readonly Color Lime_600 = new(192, 202, 51);
    public static readonly Color Lime_700 = new(175, 180, 43);
    public static readonly Color Lime_800 = new(158, 157, 36);
    public static readonly Color Lime_900 = new(130, 119, 23);
    public static readonly Color Lime_A100 = new(244, 255, 129);
    public static readonly Color Lime_A200 = new(238, 255, 65);
    public static readonly Color Lime_A400 = new(198, 255, 0);
    public static readonly Color Lime_A700 = new(174, 234, 0);

    // Yellow
    public static readonly Color Yellow_50 = new(255, 253, 231);
    public static readonly Color Yellow_100 = new(255, 249, 196);
    public static readonly Color Yellow_200 = new(255, 245, 157);
    public static readonly Color Yellow_300 = new(255, 241, 118);
    public static readonly Color Yellow_400 = new(255, 238, 88);
    public static readonly Color Yellow_500 = new(255, 235, 59);
    public static readonly Color Yellow_600 = new(253, 216, 53);
    public static readonly Color Yellow_700 = new(251, 192, 45);
    public static readonly Color Yellow_800 = new(249, 168, 37);
    public static readonly Color Yellow_900 = new(245, 127, 23);
    public static readonly Color Yellow_A100 = new(255, 255, 141);
    public static readonly Color Yellow_A200 = new(255, 255, 0);
    public static readonly Color Yellow_A400 = new(255, 234, 0);
    public static readonly Color Yellow_A700 = new(255, 214, 0);

    // Amber
    public static readonly Color Amber_50 = new(255, 248, 225);
    public static readonly Color Amber_100 = new(255, 236, 179);
    public static readonly Color Amber_200 = new(255, 224, 130);
    public static readonly Color Amber_300 = new(255, 213, 79);
    public static readonly Color Amber_400 = new(255, 202, 40);
    public static readonly Color Amber_500 = new(255, 193, 7);
    public static readonly Color Amber_600 = new(255, 179, 0);
    public static readonly Color Amber_700 = new(255, 160, 0);
    public static readonly Color Amber_800 = new(255, 143, 0);
    public static readonly Color Amber_900 = new(255, 111, 0);
    public static readonly Color Amber_A100 = new(255, 229, 127);
    public static readonly Color Amber_A200 = new(255, 215, 64);
    public static readonly Color Amber_A400 = new(255, 196, 0);
    public static readonly Color Amber_A700 = new(255, 171, 0);

    // Orange
    public static readonly Color Orange_50 = new(255, 243, 224);
    public static readonly Color Orange_100 = new(255, 224, 178);
    public static readonly Color Orange_200 = new(255, 204, 128);
    public static readonly Color Orange_300 = new(255, 183, 77);
    public static readonly Color Orange_400 = new(255, 167, 38);
    public static readonly Color Orange_500 = new(255, 152, 0);
    public static readonly Color Orange_600 = new(251, 140, 0);
    public static readonly Color Orange_700 = new(245, 124, 0);
    public static readonly Color Orange_800 = new(239, 108, 0);
    public static readonly Color Orange_900 = new(230, 81, 0);
    public static readonly Color Orange_A100 = new(255, 209, 128);
    public static readonly Color Orange_A200 = new(255, 171, 64);
    public static readonly Color Orange_A400 = new(255, 145, 0);
    public static readonly Color Orange_A700 = new(255, 109, 0);

    // Deep Orange
    public static readonly Color DeepOrange_50 = new(251, 233, 231);
    public static readonly Color DeepOrange_100 = new(255, 204, 188);
    public static readonly Color DeepOrange_200 = new(255, 171, 145);
    public static readonly Color DeepOrange_300 = new(255, 138, 101);
    public static readonly Color DeepOrange_400 = new(255, 112, 67);
    public static readonly Color DeepOrange_500 = new(255, 87, 34);
    public static readonly Color DeepOrange_600 = new(244, 81, 30);
    public static readonly Color DeepOrange_700 = new(230, 74, 25);
    public static readonly Color DeepOrange_800 = new(216, 67, 21);
    public static readonly Color DeepOrange_900 = new(191, 54, 12);
    public static readonly Color DeepOrange_A100 = new(255, 158, 128);
    public static readonly Color DeepOrange_A200 = new(255, 110, 64);
    public static readonly Color DeepOrange_A400 = new(255, 61, 0);
    public static readonly Color DeepOrange_A700 = new(221, 44, 0);

    // Brown
    public static readonly Color Brown_50 = new(239, 235, 233);
    public static readonly Color Brown_100 = new(215, 204, 200);
    public static readonly Color Brown_200 = new(188, 170, 164);
    public static readonly Color Brown_300 = new(161, 136, 127);
    public static readonly Color Brown_400 = new(141, 110, 99);
    public static readonly Color Brown_500 = new(121, 85, 72);
    public static readonly Color Brown_600 = new(109, 76, 65);
    public static readonly Color Brown_700 = new(93, 64, 55);
    public static readonly Color Brown_800 = new(78, 52, 46);
    public static readonly Color Brown_900 = new(62, 39, 35);

    // Grey
    public static readonly Color Grey_50 = new(250, 250, 250);
    public static readonly Color Grey_100 = new(245, 245, 245);
    public static readonly Color Grey_200 = new(238, 238, 238);
    public static readonly Color Grey_300 = new(224, 224, 224);
    public static readonly Color Grey_400 = new(189, 189, 189);
    public static readonly Color Grey_500 = new(158, 158, 158);
    public static readonly Color Grey_600 = new(117, 117, 117);
    public static readonly Color Grey_700 = new(97, 97, 97);
    public static readonly Color Grey_800 = new(66, 66, 66);
    public static readonly Color Grey_900 = new(33, 33, 33);

    // Blue Grey
    public static readonly Color BlueGrey_50 = new(236, 239, 241);
    public static readonly Color BlueGrey_100 = new(207, 216, 220);
    public static readonly Color BlueGrey_200 = new(176, 190, 197);
    public static readonly Color BlueGrey_300 = new(144, 164, 174);
    public static readonly Color BlueGrey_400 = new(120, 144, 156);
    public static readonly Color BlueGrey_500 = new(96, 125, 139);
    public static readonly Color BlueGrey_600 = new(84, 110, 122);
    public static readonly Color BlueGrey_700 = new(69, 90, 100);
    public static readonly Color BlueGrey_800 = new(55, 71, 79);
    public static readonly Color BlueGrey_900 = new(38, 50, 56);

    // Legacy basic color aliases for compatibility
    public static readonly Color Red = Red_500;
    public static readonly Color Green = Green_500;
    public static readonly Color Blue = Blue_500;
    public static readonly Color Yellow = Yellow_500;
    public static readonly Color Cyan = Cyan_500;
    public static readonly Color Magenta = Pink_500;
    public static readonly Color Gray = Grey_500;
}
