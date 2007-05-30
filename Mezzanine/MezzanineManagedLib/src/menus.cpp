// menus.cpp: ingame menu system (also used for scores and serverlist)

#include "cube.h"
#using <mscorlib.dll>
using namespace MezzanineLib;
using namespace MezzanineLib::Support;

struct mitem { char *text, *action; };

struct gmenu
{
    char *name;
    vector<mitem> items;
    int mwidth;
    int menusel;
};

vector<gmenu> menus;

ivector menustack;

void menuset(int menu)
{
    if((Menus::vmenu = menu)>=1) resetmovement(player1);
    if(Menus::vmenu==1) menus[1].menusel = 0;
};

void showmenu(char *name)
{
    loopv(menus) if(i>1 && strcmp(menus[i].name, name)==0)
    {
        menuset(i);
        return;
    };
};

int menucompare(mitem *a, mitem *b)
{
    int x = atoi(a->text);
    int y = atoi(b->text);
    if(x>y) return -1;
    if(x<y) return 1;
    return 0;
};

void sortmenu(int start, int num)
{
    qsort(&menus[0].items[start], num, sizeof(mitem), (int (__cdecl *)(const void *,const void *))menucompare);
};

void refreshservers();

bool rendermenu()
{
    if(Menus::vmenu<0) { menustack.setsize(0); return false; };
    if(Menus::vmenu==1) refreshservers();
    gmenu &m = menus[Menus::vmenu];
    sprintf_sd(title)(Menus::vmenu>1 ? "[ %s menu ]" : "%s", m.name);
    int mdisp = m.items.length();
    int w = 0;
    loopi(mdisp)
    {
        int x = text_width(m.items[i].text);
        if(x>w) w = x;
    };
    int tw = text_width(title);
    if(tw>w) w = tw;
    int step = GameInit::FontH/4*5;
    int h = (mdisp+2)*step;
	int y = (GameInit::VIRTH-h)/2;
    int x = (GameInit::VIRTW-w)/2;
	Render::RenderExtras::BlendBox(x-GameInit::FontH/2*3, y-GameInit::FontH, x+w+GameInit::FontH/2*3, y+h+GameInit::FontH, true);
    draw_text(title, x, y,2);
	y += GameInit::FontH*2;
    if(Menus::vmenu)
    {
        int bh = y+m.menusel*step;
        Render::RenderExtras::BlendBox(x-GameInit::FontH, bh-10, x+w+GameInit::FontH, bh+GameInit::FontH+10, false);
    };
    loopj(mdisp)
    {
        draw_text(m.items[j].text, x, y, 2);
        y += step;
    };
    return true;
};

void newmenu(char *name)
{
    gmenu &menu = menus.add();
    menu.name = newstring(name);
    menu.menusel = 0;
};

void menumanual(int m, int n, char *text)
{
    if(!n) menus[m].items.setsize(0);
    mitem &mitem = menus[m].items.add();
    mitem.text = text;
    mitem.action = "";
}

void menuitem(char *text, char *action)
{
    gmenu &menu = menus.last();
    mitem &mi = menu.items.add();
    mi.text = newstring(text);
    mi.action = action[0] ? newstring(action) : mi.text;
};

COMMAND(menuitem, FunctionSignatures::ARG_2STR);
COMMAND(showmenu, FunctionSignatures::ARG_1STR);
COMMAND(newmenu, FunctionSignatures::ARG_1STR);

bool menukey(int code, bool isdown)
{
    if(Menus::vmenu<=0) return false;
    int menusel = menus[Menus::vmenu].menusel;
    if(isdown)
    {
		if(code==(int)SdlDotNet::Input::Key::Escape)
        {
            menuset(-1);
            if(!menustack.empty()) menuset(menustack.pop());
            return true;
        }
		else if(code==(int)SdlDotNet::Input::Key::UpArrow || code==-4) menusel--;
		else if(code==(int)SdlDotNet::Input::Key::DownArrow || code==-5) menusel++;
        int n = menus[Menus::vmenu].items.length();
        if(menusel<0) menusel = n-1;
        else if(menusel>=n) menusel = 0;
        menus[Menus::vmenu].menusel = menusel;
    }
    else
    {
		if(code==(int)SdlDotNet::Input::Key::Return || code==-2)
        {
            char *action = menus[Menus::vmenu].items[menusel].action;
            if(Menus::vmenu==1) connects(getservername(menusel));
            menustack.add(Menus::vmenu);
            menuset(-1);
            execute(action, true);
        };
    };
    return true;
};
