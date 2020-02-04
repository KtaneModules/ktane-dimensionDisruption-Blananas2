using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;

public class dimensionDisruptionScript : MonoBehaviour {

    public KMBombInfo Bomb;
    public KMAudio Audio;

    public GameObject[] cubes; // A6i, B6i, C6i ... F6i, A5i, B5i ... F1i, A6ii, B6ii ... F4vi, F5vi, F6vi
    public KMSelectable[] buttons;
    public GameObject[] statusLight; //center, l&r, u&d
    public Material[] solvedGreens; //dark, light
    public GameObject[] otherLetters;
    public Sprite[] pixelLetters;

    //Logging
    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    public List<string> letterPatterns = new List<string> { ".AAAA.AAAAAAAA..AAAAAAAAAAAAAAAA..AA", "BBBBB.BB..BBBBBBB.BBBBBBBB..BBBBBBB.", ".CCCC.CCCCCCCCCC..CCCC..CCCCCC.CCCC.", "DDDDD.DDDDDDDD..DDDD..DDDDDDDDDDDDD.", "EEEEEEEEE...EEEEEEEEE...EEEEEEEEEEEE", "FFFFFFFFFFFFFFF...FFFFFFFFFFFFFFF...", ".GGGG.GGGGGGGG....GG..GGGGGGGG.GGGG.", "HH..HHHH..HHHHHHHHHHHHHHHH..HHHH..HH", "IIIIIIIIIIII..II....II..IIIIIIIIIIII", "JJJJJJJJJJJJ..JJ....JJ..JJJJ..JJJ...", "KK..KKKK..KKKKKKK.KKKKKKKK..KKKK..KK", "LLL...LLL...LLL...LLLLLLLLLLLLLLLLLL", "M....MMM..MMMMMMMMMMMMMMMM..MMMM..MM", "NN..NNNNN.NNNNNNNNNNNNNNNN.NNNNN..NN", ".OOOO.OOOOOOOO..OOOO..OOOOOOOO.OOOO.", "PPPPP.PP..PPPPPPPPPPPPP.PP....PP....", ".QQQQ.QQQQQQQQ..QQQQ.QQQQQQQQQ.QQQQQ", "RRRRR.RR..RRRRRRRRRRRRR.RRRRRRRR..RR", "SSSSSSSS..SSSSS......SSSSS..SSSSSSSS", "TTTTTTTTTTTTTTTTTT.TTTT..TTTT..TTTT.", "UU..UUUU..UUUU..UUUUUUUUUUUUUU.UUUU.", "VV..VVVV..VVVV..VVVVVVVV.VVVV..VVVV.", "WW..WWWW..WWWWWWWWWWWWWWWW..WWW....W", "XX..XXXX..XX.XXXX..XXXX.XX..XXXX..XX", "YY..YYYY..YYYYYYYY.YYYY...YY....YY..", "ZZZZZZZZZZZZ..ZZZ..ZZZ..ZZZZZZZZZZZZ", ".0000.00000000.000000.00000000.0000.", "11111.11111..1111..1111.111111111111", ".2222.22..22...222.2222.222...222222", "3333333...33..333...333.3...33333333", "44..4444..44444444444444....44....44", "55555555....55555.....5555555555555.", ".6666666....66666.66..66666666.6666.", "777777777777...777..777..777...777..", ".8888.88..88.8888..8888.88..88.8888.", ".9999.99999999..99.99999....9999999."
 };
    public List<int> cubePlaces = new List<int> { 30, 0, 35, 31, 1, 35, 32, 2, 35, 33, 3, 35, 34, 4, 35, 35, 5, 35, 24, 0, 29, 25, 1, 29, 26, 2, 29, 27, 3, 29, 28, 4, 29, 29, 5, 29, 18, 0, 23, 19, 1, 23, 20, 2, 23, 21, 3, 23, 22, 4, 23, 23, 5, 23, 12, 0, 17, 13, 1, 17, 14, 2, 17, 15, 3, 17, 16, 4, 17, 17, 5, 17, 6, 0, 11, 7, 1, 11, 8, 2, 11, 9, 3, 11, 10, 4, 11, 11, 5, 11, 0, 0, 5, 1, 1, 5, 2, 2, 5, 3, 3, 5, 4, 4, 5, 5, 5, 5, 30, 6, 34, 31, 7, 34, 32, 8, 34, 33, 9, 34, 34, 10, 34, 35, 11, 34, 24, 6, 28, 25, 7, 28, 26, 8, 28, 27, 9, 28, 28, 10, 28, 29, 11, 28, 18, 6, 22, 19, 7, 22, 20, 8, 22, 21, 9, 22, 22, 10, 22, 23, 11, 22, 12, 6, 16, 13, 7, 16, 14, 8, 16, 15, 9, 16, 16, 10, 16, 17, 11, 16, 6, 6, 10, 7, 7, 10, 8, 8, 10, 9, 9, 10, 10, 10, 10, 11, 11, 10, 0, 6, 4, 1, 7, 4, 2, 8, 4, 3, 9, 4, 4, 10, 4, 5, 11, 4, 30, 12, 33, 31, 13, 33, 32, 14, 33, 33, 15, 33, 34, 16, 33, 35, 17, 33, 24, 12, 27, 25, 13, 27, 26, 14, 27, 27, 15, 27, 28, 16, 27, 29, 17, 27, 18, 12, 21, 19, 13, 21, 20, 14, 21, 21, 15, 21, 22, 16, 21, 23, 17, 21, 12, 12, 15, 13, 13, 15, 14, 14, 15, 15, 15, 15, 16, 16, 15, 17, 17, 15, 6, 12, 9, 7, 13, 9, 8, 14, 9, 9, 15, 9, 10, 16, 9, 11, 17, 9, 0, 12, 3, 1, 13, 3, 2, 14, 3, 3, 15, 3, 4, 16, 3, 5, 17, 3, 30, 18, 32, 31, 19, 32, 32, 20, 32, 33, 21, 32, 34, 22, 32, 35, 23, 32, 24, 18, 26, 25, 19, 26, 26, 20, 26, 27, 21, 26, 28, 22, 26, 29, 23, 26, 18, 18, 20, 19, 19, 20, 20, 20, 20, 21, 21, 20, 22, 22, 20, 23, 23, 20, 12, 18, 14, 13, 19, 14, 14, 20, 14, 15, 21, 14, 16, 22, 14, 17, 23, 14, 6, 18, 8, 7, 19, 8, 8, 20, 8, 9, 21, 8, 10, 22, 8, 11, 23, 8, 0, 18, 2, 1, 19, 2, 2, 20, 2, 3, 21, 2, 4, 22, 2, 5, 23, 2, 30, 24, 31, 31, 25, 31, 32, 26, 31, 33, 27, 31, 34, 28, 31, 35, 29, 31, 24, 24, 25, 25, 25, 25, 26, 26, 25, 27, 27, 25, 28, 28, 25, 29, 29, 25, 18, 24, 19, 19, 25, 19, 20, 26, 19, 21, 27, 19, 22, 28, 19, 23, 29, 19, 12, 24, 13, 13, 25, 13, 14, 26, 13, 15, 27, 13, 16, 28, 13, 17, 29, 13, 6, 24, 7, 7, 25, 7, 8, 26, 7, 9, 27, 7, 10, 28, 7, 11, 29, 7, 0, 24, 1, 1, 25, 1, 2, 26, 1, 3, 27, 1, 4, 28, 1, 5, 29, 1, 30, 30, 30, 31, 31, 30, 32, 32, 30, 33, 33, 30, 34, 34, 30, 35, 35, 30, 24, 30, 24, 25, 31, 24, 26, 32, 24, 27, 33, 24, 28, 34, 24, 29, 35, 24, 18, 30, 18, 19, 31, 18, 20, 32, 18, 21, 33, 18, 22, 34, 18, 23, 35, 18, 12, 30, 12, 13, 31, 12, 14, 32, 12, 15, 33, 12, 16, 34, 12, 17, 35, 12, 6, 30, 6, 7, 31, 6, 8, 32, 6, 9, 33, 6, 10, 34, 6, 11, 35, 6, 0, 30, 0, 1, 31, 0, 2, 32, 0, 3, 33, 0, 4, 34, 0, 5, 35, 0 };
    public string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    public string table = "1UZ4W3Y05V2X7T6AQRMNIDFEGCHJLKPBOS98";
    public int horiz = -1;
    public int verti = -1;
    public int letOne = -1;
    public int letTwo = -1;
    public int letThree = -1;
    public List<int> tablePlaces = new List<int> {  };
    public List<int> corners = new List<int> { 0, 1, 2, 3 };
    public List<string> cornerNames = new List<string> { "Top left", "Top right", "Bottom left", "Bottom right" };

    void Awake () {
        moduleId = moduleIdCounter++;
        foreach (KMSelectable button in buttons) {
            KMSelectable pressedButton = button;
            button.OnInteract += delegate () { buttonPress(pressedButton); return false; };
        }

    }

    // Use this for initialization
    void Start () {
        tablePlaces.Add(UnityEngine.Random.Range(0, 36));
        horiz = (UnityEngine.Random.Range(0, 5)) + 1;
        verti = (UnityEngine.Random.Range(0, 5)) + 1;
        tablePlaces.Add((tablePlaces[0] + (6 * verti) + 36) % 36);
        if ((tablePlaces[0] % 6) + horiz > 5) {
            tablePlaces.Add(tablePlaces[0] - (6 - horiz));
            tablePlaces.Add(tablePlaces[1] - (6 - horiz));
        } else {
            tablePlaces.Add(tablePlaces[0] + horiz);
            tablePlaces.Add(tablePlaces[1] + horiz);
        }
        tablePlaces.Sort();

        corners.Shuffle();

        letOne = alphabet.IndexOf(table[tablePlaces[corners[0]]]);
        letTwo = alphabet.IndexOf(table[tablePlaces[corners[1]]]);
        letThree = alphabet.IndexOf(table[tablePlaces[corners[2]]]);

        otherLetters[0].GetComponent<SpriteRenderer>().sprite = pixelLetters[letOne];
        otherLetters[1].GetComponent<SpriteRenderer>().sprite = pixelLetters[letTwo];
        otherLetters[2].GetComponent<SpriteRenderer>().sprite = pixelLetters[letThree];

        Debug.LogFormat("[Dimension Disruption #{0}] Chosen letters: {1}, {2}, {3}.", moduleId, alphabet[letOne], alphabet[letTwo], alphabet[letThree]);

        for (int i = 0; i < 216; i++) {
            if ((letterPatterns[letOne][cubePlaces[i * 3]] == '.' || letterPatterns[letTwo][cubePlaces[(i * 3) + 1]] == '.') || letterPatterns[letThree][cubePlaces[(i * 3) + 2]] == '.') {
                cubes[i].SetActive(false);
            } else {
                continue;
            }
        }
	}

    void buttonPress(KMSelectable button) {
        if (moduleSolved == false) {
            for (int j = 0; j < 4; j++) {
                if (buttons[j] == button) {
                    if (corners[3] == j) {
                        GetComponent<KMBombModule>().HandlePass();
                        Debug.LogFormat("[Dimension Disruption #{0}] {1} button pressed, which is correct. Module solved.", moduleId, cornerNames[j] );
                        statusLight[0].GetComponent<MeshRenderer>().material = solvedGreens[0];
                        statusLight[1].GetComponent<MeshRenderer>().material = solvedGreens[1];
                        statusLight[2].GetComponent<MeshRenderer>().material = solvedGreens[1];
                        moduleSolved = true;
                    } else {
                    GetComponent<KMBombModule>().HandleStrike();
                    Debug.LogFormat("[Dimension Disruption #{0}] {1} button pressed, which is incorrect. Module striked.", moduleId, cornerNames[j] );
                    }
                }
            }
        }
    }

}
