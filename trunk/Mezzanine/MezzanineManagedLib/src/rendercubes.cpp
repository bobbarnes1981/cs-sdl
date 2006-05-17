// rendercubes.cpp: sits in between worldrender.cpp and rendergl.cpp and fills the vertex array for different cube surfaces.

#include "cube.h"
#using <mscorlib.dll>
using namespace MezzanineLib;
using namespace MezzanineLib::Render;

vertex *verts = NULL;

void setarraypointers()
{
    glVertexPointer(3, GL_FLOAT, sizeof(vertex), &verts[0].x);
    glColorPointer(4, GL_UNSIGNED_BYTE, sizeof(vertex), &verts[0].r);
    glTexCoordPointer(2, GL_FLOAT, sizeof(vertex), &verts[0].u);
};

void reallocv()
{
    verts = (vertex *)realloc(verts, (RenderCubes::curmaxverts *= 2)*sizeof(vertex));
    RenderCubes::curmaxverts -= 10;
    if(!verts) MezzanineLib::GameInit::Fatal("no vertex memory!");
    setarraypointers();
};

// generating the actual vertices is done dynamically every frame and sits at the
// leaves of all these functions, and are part of the cpu bottleneck on really slow
// machines, hence the macros.

#define vertcheck() { if(RenderCubes::curvert>=RenderCubes::curmaxverts) reallocv(); }

#define vertf(v1, v2, v3, ls, t1, t2) { vertex &v = verts[RenderCubes::curvert++]; \
    v.u = t1; v.v = t2; \
    v.x = v1; v.y = v2; v.z = v3; \
    v.r = ls->r; v.g = ls->g; v.b = ls->b; v.a = 255; };

#define vert(v1, v2, v3, ls, t1, t2) { vertf((float)(v1), (float)(v2), (float)(v3), ls, t1, t2); }

void showmip() { RenderCubes::showmip(); };
void mipstats(int a, int b, int c) { if(RenderCubes::showm) conoutf("1x1/2x2/4x4: %d / %d / %d", a, b, c); };

COMMAND(showmip, MezzanineLib::Support::FunctionSignatures::ARG_NONE);

#define stripend() { if(RenderCubes::floorstrip || RenderCubes::deltastrip) { addstrip(RenderCubes::ogltex, RenderCubes::firstindex, RenderCubes::curvert-RenderCubes::firstindex); RenderCubes::floorstrip = RenderCubes::deltastrip = false; }; };
void finishstrips() { stripend(); };

sqr sbright, sdark;
VAR(lighterror,1,8,100);

void render_flat(int wtex, int x, int y, int size, int h, sqr *l1, sqr *l2, sqr *l3, sqr *l4, bool isceil)  // floor/ceil quads
{
    vertcheck();
    if(RenderCubes::showm) { l3 = l1 = &sbright; l4 = l2 = &sdark; };

    int sx, sy;
    int gltex = Render::RenderGl::LookupTexture(wtex, &sx, &sy);
    float xf = RenderCubes::TEXTURESCALE/sx;
    float yf = RenderCubes::TEXTURESCALE/sy;
    float xs = size*xf;
    float ys = size*yf;
    float xo = xf*x;
    float yo = yf*y;

    bool first = !RenderCubes::floorstrip || y!=RenderCubes::oy+size || RenderCubes::ogltex!=gltex || h!=RenderCubes::oh || x!=RenderCubes::ox;

    if(first)       // start strip here
    {
        stripend();
        RenderCubes::firstindex = RenderCubes::curvert;
        RenderCubes::ogltex = gltex;
        RenderCubes::oh = h;
        RenderCubes::ox = x;
        RenderCubes::floorstrip = true;
        if(isceil)
        {
            vert(x+size, h, y, l2, xo+xs, yo);
            vert(x,      h, y, l1, xo, yo);
        }
        else
        {
            vert(x,      h, y, l1, xo,    yo);
            vert(x+size, h, y, l2, xo+xs, yo);
        };
        RenderCubes::ol3r = l1->r;
        RenderCubes::ol3g = l1->g;
        RenderCubes::ol3b = l1->b;
        RenderCubes::ol4r = l2->r;
        RenderCubes::ol4g = l2->g;
        RenderCubes::ol4b = l2->b;
    }
    else        // continue strip
    {
        int lighterr = lighterror*2;
        if((abs(RenderCubes::ol3r-l3->r)<lighterr && abs(RenderCubes::ol4r-l4->r)<lighterr        // skip vertices if light values are close enough
        &&  abs(RenderCubes::ol3g-l3->g)<lighterr && abs(RenderCubes::ol4g-l4->g)<lighterr
        &&  abs(RenderCubes::ol3b-l3->b)<lighterr && abs(RenderCubes::ol4b-l4->b)<lighterr) || !wtex)   
        {
            RenderCubes::curvert -= 2;
            RenderCubes::nquads--;
        }
        else
        {
            uchar *p3 = (uchar *)(&verts[RenderCubes::curvert-1].r);
            RenderCubes::ol3r = p3[0];  
            RenderCubes::ol3g = p3[1];  
            RenderCubes::ol3b = p3[2];
            uchar *p4 = (uchar *)(&verts[RenderCubes::curvert-2].r);  
            RenderCubes::ol4r = p4[0];
            RenderCubes::ol4g = p4[1];
            RenderCubes::ol4b = p4[2];
        };
    };

    if(isceil)
    {
        vert(x+size, h, y+size, l3, xo+xs, yo+ys);
        vert(x,      h, y+size, l4, xo,    yo+ys); 
    }
    else
    {
        vert(x,      h, y+size, l4, xo,    yo+ys);
        vert(x+size, h, y+size, l3, xo+xs, yo+ys); 
    };

    RenderCubes::oy = y;
    RenderCubes::nquads++;
};

void render_flatdelta(int wtex, int x, int y, int size, float h1, float h2, float h3, float h4, sqr *l1, sqr *l2, sqr *l3, sqr *l4, bool isceil)  // floor/ceil quads on a slope
{
    vertcheck();
    if(RenderCubes::showm) { l3 = l1 = &sbright; l4 = l2 = &sdark; };

    int sx, sy;
    int gltex = Render::RenderGl::LookupTexture(wtex, &sx, &sy);
    float xf = RenderCubes::TEXTURESCALE/sx;
    float yf = RenderCubes::TEXTURESCALE/sy;
    float xs = size*xf;
    float ys = size*yf;
    float xo = xf*x;
    float yo = yf*y;

    bool first = !RenderCubes::deltastrip || y!=RenderCubes::oy+size || RenderCubes::ogltex!=gltex || x!=RenderCubes::ox; 

    if(first) 
    {
        stripend();
        RenderCubes::firstindex = RenderCubes::curvert;
        RenderCubes::ogltex = gltex;
        RenderCubes::ox = x;
        RenderCubes::deltastrip = true;
        if(isceil)
        {
            vertf((float)x+size, h2, (float)y,      l2, xo+xs, yo);
            vertf((float)x,      h1, (float)y,      l1, xo,    yo);
        }
        else
        {
            vertf((float)x,      h1, (float)y,      l1, xo,    yo);
            vertf((float)x+size, h2, (float)y,      l2, xo+xs, yo);
        };
        RenderCubes::ol3r = l1->r;
        RenderCubes::ol3g = l1->g;
        RenderCubes::ol3b = l1->b;
        RenderCubes::ol4r = l2->r;
        RenderCubes::ol4g = l2->g;
        RenderCubes::ol4b = l2->b;
    };

    if(isceil)
    {
        vertf((float)x+size, h3, (float)y+size, l3, xo+xs, yo+ys); 
        vertf((float)x,      h4, (float)y+size, l4, xo,    yo+ys);
    }
    else
    {
        vertf((float)x,      h4, (float)y+size, l4, xo,    yo+ys);
        vertf((float)x+size, h3, (float)y+size, l3, xo+xs, yo+ys); 
    };

    RenderCubes::oy = y;
    RenderCubes::nquads++;
};

void render_2tris(sqr *h, sqr *s, int x1, int y1, int x2, int y2, int x3, int y3, sqr *l1, sqr *l2, sqr *l3)   // floor/ceil tris on a corner cube
{
    stripend();
    vertcheck();

    int sx, sy;
    int gltex = Render::RenderGl::LookupTexture(h->ftex, &sx, &sy);
    float xf = RenderCubes::TEXTURESCALE/sx;
    float yf = RenderCubes::TEXTURESCALE/sy;

    vertf((float)x1, h->floor, (float)y1, l1, xf*x1, yf*y1);
    vertf((float)x2, h->floor, (float)y2, l2, xf*x2, yf*y2);
    vertf((float)x3, h->floor, (float)y3, l3, xf*x3, yf*y3);
    addstrip(gltex, RenderCubes::curvert-3, 3);

    gltex = Render::RenderGl::LookupTexture(h->ctex, &sx, &sy);
    xf = RenderCubes::TEXTURESCALE/sx;
    yf = RenderCubes::TEXTURESCALE/sy;

    vertf((float)x3, h->ceil, (float)y3, l3, xf*x3, yf*y3);
    vertf((float)x2, h->ceil, (float)y2, l2, xf*x2, yf*y2);
    vertf((float)x1, h->ceil, (float)y1, l1, xf*x1, yf*y1);
    addstrip(gltex, RenderCubes::curvert-3, 3);
    RenderCubes::nquads++;
};

void render_tris(int x, int y, int size, bool topleft,
                 sqr *h1, sqr *h2, sqr *s, sqr *t, sqr *u, sqr *v)
{
    if(topleft)
    {
        if(h1) render_2tris(h1, s, x+size, y+size, x, y+size, x, y, u, v, s);
        if(h2) render_2tris(h2, s, x, y, x+size, y, x+size, y+size, s, t, v);
    }
    else
    {
        if(h1) render_2tris(h1, s, x, y, x+size, y, x, y+size, s, t, u);
        if(h2) render_2tris(h2, s, x+size, y, x+size, y+size, x, y+size, t, u, v);
    };
};

void render_square(int wtex, float floor1, float floor2, float ceil1, float ceil2, int x1, int y1, int x2, int y2, int size, sqr *l1, sqr *l2, bool flip)   // wall quads
{
    stripend();
    vertcheck();
    if(RenderCubes::showm) { l1 = &sbright; l2 = &sdark; };

    int sx, sy;
    int gltex = Render::RenderGl::LookupTexture(wtex, &sx, &sy);
    float xf = RenderCubes::TEXTURESCALE/sx;
    float yf = RenderCubes::TEXTURESCALE/sy;
    float xs = size*xf;
    float xo = xf*(x1==x2 ? min(y1,y2) : min(x1,x2));

    if(!flip)
    {
        vertf((float)x2, ceil2,  (float)y2, l2, xo+xs, -yf*ceil2);
        vertf((float)x1, ceil1,  (float)y1, l1, xo,    -yf*ceil1); 
        vertf((float)x2, floor2, (float)y2, l2, xo+xs, -floor2*yf); 
        vertf((float)x1, floor1, (float)y1, l1, xo,    -floor1*yf); 
    }
    else
    {
        vertf((float)x1, ceil1,  (float)y1, l1, xo,    -yf*ceil1);
        vertf((float)x2, ceil2,  (float)y2, l2, xo+xs, -yf*ceil2); 
        vertf((float)x1, floor1, (float)y1, l1, xo,    -floor1*yf); 
        vertf((float)x2, floor2, (float)y2, l2, xo+xs, -floor2*yf); 
    };

    RenderCubes::nquads++;
    addstrip(gltex, RenderCubes::curvert-4, 4);
};

VAR(watersubdiv, 1, 4, 64);
VARF(waterlevel, -128, -128, 127, if(!noteditmode()) hdr.waterlevel = waterlevel);

inline void vertw(int v1, float v2, int v3, sqr *c, float t1, float t2, float t)
{
    vertcheck();
    vertf((float)v1, v2-(float)sin(v1*v3*0.1+t)*0.2f, (float)v3, c, t1, t2);
};

// renders water for bounding rect area that contains water... simple but very inefficient

int renderwater(float hf)
{
    if(RenderCubes::wx1<0) return RenderCubes::nquads;

    glDepthMask(GL_FALSE);
    glEnable(GL_BLEND);
    glBlendFunc(GL_ONE, GL_SRC_COLOR);
    int sx, sy;
    glBindTexture(GL_TEXTURE_2D, Render::RenderGl::LookupTexture(MezzanineLib::TextureNumbers::DEFAULT_LIQUID, &sx, &sy));  

    RenderCubes::wx1 &= ~(watersubdiv-1);
    RenderCubes::wy1 &= ~(watersubdiv-1);

    float xf = RenderCubes::TEXTURESCALE/sx;
    float yf = RenderCubes::TEXTURESCALE/sy;
    float xs = watersubdiv*xf;
    float ys = watersubdiv*yf;
    float t1 = MezzanineLib::GameInit::LastMillis/300.0f;
    float t2 = MezzanineLib::GameInit::LastMillis/4000.0f;
    
    sqr dl;
    dl.r = dl.g = dl.b = 255;
    
    for(int xx = RenderCubes::wx1; xx<RenderCubes::wx2; xx += watersubdiv)
    {
        for(int yy = RenderCubes::wy1; yy<RenderCubes::wy2; yy += watersubdiv)
        {
            float xo = xf*(xx+t2);
            float yo = yf*(yy+t2);
            if(yy==RenderCubes::wy1)
            {
                vertw(xx,             hf, yy,             &dl, RenderCubes::dx(xo),    RenderCubes::dy(yo), t1);
                vertw(xx+watersubdiv, hf, yy,             &dl, RenderCubes::dx(xo+xs), RenderCubes::dy(yo), t1);
            };
            vertw(xx,             hf, yy+watersubdiv, &dl, RenderCubes::dx(xo),    RenderCubes::dy(yo+ys), t1);
            vertw(xx+watersubdiv, hf, yy+watersubdiv, &dl, RenderCubes::dx(xo+xs), RenderCubes::dy(yo+ys), t1); 
        };   
        int n = (RenderCubes::wy2-RenderCubes::wy1-1)/watersubdiv;
        RenderCubes::nquads += n;
        n = (n+2)*2;
        glDrawArrays(GL_TRIANGLE_STRIP, RenderCubes::curvert -= n, n);
    };
    
    glDisable(GL_BLEND);
    glDepthMask(GL_TRUE);
    
    return RenderCubes::nquads;
};

void resetcubes()
{
    if(!verts) reallocv();
    RenderCubes::floorstrip = RenderCubes::deltastrip = false;
    RenderCubes::wx1 = -1;
    RenderCubes::nquads = 0;
    sbright.r = sbright.g = sbright.b = 255;
    sdark.r = sdark.g = sdark.b = 0;
};


