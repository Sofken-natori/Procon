using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace state
{

    public enum State
    {
        Empty = 0, // ��������
        Red = 1, // ��
        Blue = 2, // ��
        castle = 3,//��
        lake = 4,//�r
        jinti_Red = 5, // �w�n
        jinti_Bule = 6,
        Wall = 7,
        KomaRed = 8,
        KomaBlue = 9,
        KomaMoveLeft = 10,
        KomaMoveRight = 11,
        KomaMoveUp = 12,
        KomaMoveDown = 13,
        KomaMoveUpLeft = 14,
        KomaMoveDownLeft = 15,
        KomaMoveUpRight = 16,
        KomaMoveDownRight = 17,
        MoveDefolt = 18,
    }


}
