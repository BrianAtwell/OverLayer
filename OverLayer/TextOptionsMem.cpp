/*
File: TextOptionsMem.cpp
Author: Brian Atwell
Date: November 24, 2019
Descriptions: Preforms memory operations on TextOptions Struct
*/
#include "stdafx.h"
#include "TextOptionsMem.h"

TEXTOPTIONS textOptsArray[MAX_TEXTOPTIONS] = { 0 };
int textOptsLen = 0;

bool AddTextOptions(TEXTOPTIONS* textOpts)
{
	if (textOpts != NULL && textOptsLen < MAX_TEXTOPTIONS)
	{
		textOptsArray[textOptsLen] = *textOpts;
#if TEXTOPTIONS == TEXTOPTIONS_W
		textOptsArray[textOptsLen].cStr = (wchar_t*)malloc(textOptsArray[textOptsLen].strLen*sizeof(textOptsArray[textOptsLen].cStr));
#elif TEXTOPTIONS == TEXTOPTIONS_A
		textOptsArray[textOptsLen].cStr = (char*)malloc(textOptsArray[textOptsLen].strLen*sizeof(textOptsArray[textOptsLen].cStr));
#endif
		if (textOptsArray[textOptsLen].cStr == 0)
		{
			return false;
		}
		memcpy(textOptsArray[textOptsLen].cStr, textOpts->cStr, textOpts->strLen*sizeof(textOptsArray[textOptsLen].cStr));
		return true;
	}

	return false;
}

bool RemoveLastTextOptions()
{

	return false;
}

void FindTextOptionsByString(char* textStr, int len, TEXTOPTIONS* textOpts);

void TextOptionsAtPos(int pos, TEXTOPTIONS* textOpts);

int TextOptionsLength();
