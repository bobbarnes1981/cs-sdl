// console.cpp: the console buffer, its display, and command line control

#include "cube.h"
#include <ctype.h>
#using <mscorlib.dll>
using namespace MezzanineLib;
using namespace MezzanineLib::Support;

struct cline { char *cref; int outtime; };
vector<cline> conlines;

string commandbuf;

void setconskip(int n)
{
    Console::conskip += n;
    if(Console::conskip<0) Console::conskip = 0;
};

COMMANDN(Console::conskip, setconskip, FunctionSignatures::ARG_1INT);

void conline(const char *sf, bool highlight)        // add a line to the console buffer
{
    cline cl;
    cl.cref = conlines.length()>100 ? conlines.pop().cref : newstringbuf("");   // constrain the buffer size
    cl.outtime = GameInit::LastMillis;                        // for how long to keep line on screen
    conlines.insert(0,cl);
    if(highlight)                                   // show line in a different colour, for chat etc.
    {
        cl.cref[0] = '\f';
        cl.cref[1] = 0;
        strcat_s(cl.cref, sf);
    }
    else
    {
        strcpy_s(cl.cref, sf);
    };
    puts(cl.cref);
};

void conoutf(const char *s, ...)
{
    sprintf_sdv(sf, s);
    s = sf;
    int n = 0;
    //while(strlen(s)>Console::WORDWRAP)                       // cut strings to fit on screen
    //{
    //    string t;
    //    strn0cpy(t, s, Console::WORDWRAP+1);
    //    conline(t, n++!=0);
    //    s += Console::WORDWRAP;
    //};
    conline(s, n!=0);
};

void renderconsole()                                // render buffer taking into account time & scrolling
{
    int nd = 0;
    char *refs[Console::ndraw];
    loopv(conlines) if(Console::conskip ? i>=Console::conskip-1 || i>=conlines.length()-Console::ndraw : GameInit::LastMillis-conlines[i].outtime<20000)
    {
        refs[nd++] = conlines[i].cref;
        if(nd==Console::ndraw) break;
    };
    loopj(nd)
    {
        draw_text(refs[j], GameInit::FontH/3, (GameInit::FontH/4*5)*(nd-j-1)+GameInit::FontH/3, 2);
    };
};

// keymap is defined externally in keymap.cfg

struct keym { int code; char *name; char *action; } keyms[256];                                    

void keymap(char *code, char *key, char *action)
{
    keyms[Console::numkm].code = atoi(code);
    keyms[Console::numkm].name = newstring(key);
    keyms[Console::numkm++].action = newstringbuf(action);
};

COMMAND(keymap, FunctionSignatures::ARG_3STR);

void bindkey(char *key, char *action)
{
    for(char *x = key; *x; x++) *x = toupper(*x);
    loopi(Console::numkm) if(strcmp(keyms[i].name, key)==0)
    {
        strcpy_s(keyms[i].action, action);
        return;
    };
    conoutf("unknown key \"%s\"", key);   
};

COMMANDN(bind, bindkey, FunctionSignatures::ARG_2STR);

void saycommand(char *init)                         // turns input to the command line on or off
{
	SdlDotNet::Input::Keyboard::UnicodeEnabled = (Console::saycommandon = (init!=NULL));
    if(!GameInit::EditMode) SdlDotNet::Input::Keyboard::KeyRepeat = Console::saycommandon;
    if(!init) init = "";
    strcpy_s(commandbuf, init);
};

void mapmsg(char *s) { strn0cpy(hdr.maptitle, s, 128); };

COMMAND(saycommand, FunctionSignatures::ARG_VARI);
COMMAND(mapmsg, FunctionSignatures::ARG_1STR);

void pasteconsole()
{
    if(!IsClipboardFormatAvailable(CF_TEXT)) return; 
    if(!OpenClipboard(NULL)) return;
    char *cb = (char *)GlobalLock(GetClipboardData(CF_TEXT));
    strcat_s(commandbuf, cb);
    GlobalUnlock(cb);
    CloseClipboard();
};

cvector vhistory;

void history(int n)
{
    static bool rec = false;
    if(!rec && n>=0 && n<vhistory.length())
    {
        rec = true;
        execute(vhistory[vhistory.length()-n-1]);
        rec = false;
    };
};

COMMAND(history, FunctionSignatures::ARG_1INT);

void keypress(int code, bool isdown, int cooked)
{
    if(Console::saycommandon)                                // keystrokes go to commandline
    {
        if(isdown)
        {
            switch(code)
            {
			case (int)SdlDotNet::Input::Key::Return:
                    break;

				case (int)SdlDotNet::Input::Key::Backspace:
				case (int)SdlDotNet::Input::Key::LeftArrow:
                {
                    for(int i = 0; commandbuf[i]; i++) if(!commandbuf[i+1]) commandbuf[i] = 0;
                    Command::ResetComplete();
                    break;
                };
                    
				case (int)SdlDotNet::Input::Key::UpArrow:
                    if(Console::histpos) strcpy_s(commandbuf, vhistory[--Console::histpos]);
                    break;
                
				case (int)SdlDotNet::Input::Key::DownArrow:
                    if(Console::histpos<vhistory.length()) strcpy_s(commandbuf, vhistory[Console::histpos++]);
                    break;
                    
				case (int)SdlDotNet::Input::Key::Tab:
                    complete(commandbuf);
                    break;

				case (int)SdlDotNet::Input::Key::V:
                    //if(SDL_GetModState()&(KMOD_LCTRL|KMOD_RCTRL)) { pasteconsole(); return; };
					if(SdlDotNet::Input::Keyboard::ModifierKeyState & (SdlDotNet::Input::Key::LeftControl|SdlDotNet::Input::Key::RightControl)) 
					{ 
						pasteconsole(); 
						return; 
					};

                default:
                    Command::ResetComplete();
                    if(cooked) { char add[] = { cooked, 0 }; strcat_s(commandbuf, add); };
            };
        }
        else
        {
			if(code==(int)SdlDotNet::Input::Key::Return)
            {
                if(commandbuf[0])
                {
                    if(vhistory.empty() || strcmp(vhistory.last(), commandbuf))
                    {
                        vhistory.add(newstring(commandbuf));  // cap this?
                    };
                    Console::histpos = vhistory.length();
                    if(commandbuf[0]=='/') execute(commandbuf, true);
                    else toserver(commandbuf);
                };
                saycommand(NULL);
            }
            else if(code==(int)SdlDotNet::Input::Key::Escape)
            {
                saycommand(NULL);
            };
        };
    }
    else if(!menukey(code, isdown))                 // keystrokes go to menu
    {
        loopi(Console::numkm) if(keyms[i].code==code)        // keystrokes go to game, lookup in keymap and execute
        {
            string temp;
            strcpy_s(temp, keyms[i].action);
            execute(temp, isdown); 
            return;
        };
    };
};

char *getcurcommand()
{
    return Console::saycommandon ? commandbuf : NULL;
};

void writebinds(FILE *f)
{
    loopi(Console::numkm)
    {
        if(*keyms[i].action) fprintf(f, "bind \"%s\" [%s]\n", keyms[i].name, keyms[i].action);
    };
};