/*
 * TestRenderer.java
 * Copyright (C) 2003
 *
 * $Id: TestRenderer.java,v 1.9 2008/03/02 14:56:21 cawe Exp $
 */
/*
 Copyright (C) 1997-2001 Id Software, Inc.
 
 This program is free software; you can redistribute it and/or
 modify it under the terms of the GNU General Public License
 as published by the Free Software Foundation; either version 2
 of the License, or (at your option) any later version.
 
 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 
 See the GNU General Public License for more details.
 
 You should have received a copy of the GNU General Public License
 along with this program; if not, write to the Free Software
 Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 
 */
package jake2.render;

import jake2.Defines;
import jake2.Globals;
import jake2.Jake2;
import jake2.client.*;
import jake2.game.Cmd;
import jake2.qcommon.*;
import jake2.sys.KBD;
import jake2.util.Lib;
import jake2.util.Math3D;

import java.awt.Dimension;
import java.nio.FloatBuffer;
import java.util.*;

/**
 * TestRenderer
 * 
 * @author cwei
 */
public class TestRenderer {
    
    String[] args;
    
    refexport_t re;
    
    viddef_t viddef;
    
    int framecount = 0;
    
    static int testnr = 0;
    
    public TestRenderer(String[] args) {
        this.args = args;
    }
    
    public static void main(String[] args) {
        
        TestRenderer test = new TestRenderer(args);
        test.init();
        test.run();
    }
    
    KBD kbd;
    
    void init() {
	Globals.dedicated = Cvar.Get("dedicated", "0", Qcommon.CVAR_NOSET);
	Jake2.Q2Dialog = new Q2DataDialog();
	Locale.setDefault(Locale.US);
	Jake2.Q2Dialog.setVisible(true);

	String DRIVER = "jsr231";

	Qcommon.Init(new String[] { "TestRenderer", "+set", "gl_mode", "6",
		"+set", "vid_fullscreen", "0", "+set", "vid_ref", DRIVER });
	// sehr wichtig !!!
	VID.Shutdown();

	String[] names = Renderer.getDriverNames();
	System.out.println("Registered Drivers: " + Arrays.asList(names));

	this.re = Renderer.getDriver(DRIVER);
	Globals.re = this.re;

	System.out.println("Use driver: " + re);
	System.out.flush();

	re.Init(0, 0);
	kbd = re.getKeyboardHandler();
	kbd.Init();

	Cmd.AddCommand("nexttest", nexttest);
	Cbuf.AddText("bind n nexttest");
	Cbuf.Execute();
	Globals.cls.key_dest = Defines.key_game;
	Globals.cls.state = Defines.ca_active;
    }
    
    float fps = 0.0f;
    
    long start = 0;
    
    void updateScreen() {
        re.BeginFrame(0.0f);
        viddef = Globals.viddef;
        re.DrawStretchPic(0, 0, viddef.getWidth(), viddef.getHeight(), "conback");
        
        if (framecount % 500 == 0) {
            long time = System.currentTimeMillis();
            fps = 500000.0f / (time - start);
            start = time;
        }
        String text = ((int)(fps +0.5f)) + " fps";
        
        for (int i = 0; i < text.length(); i++) {
            re.DrawChar(10 + 8 * i, viddef.getHeight() / 2, (int) text.charAt(i));
        }
        
        Dimension wal = new Dimension();
        re.DrawGetPicSize(wal, "/textures/e1u1/basemap.wal");
        
        re.DrawPic(0, viddef.getHeight() - wal.height, "/textures/e1u1/basemap.wal");
        
        switch (testnr) {
        case 0:
            testParticles();
            break;
        case 1:
            testModel();
            break;
        case 2:
            testSprites();
            break;
        case 3:
            testBeam();
        }
        re.EndFrame();
        framecount++;
    }
    
    long startTime;
    
    void run() {
        startTime = System.currentTimeMillis();
        xcommand_t callback = new xcommand_t() {
            public void execute() {
                updateScreen();
            }
        };
        while (true) {
            re.updateScreen(callback);
            kbd.Update();
            Cbuf.Execute();
            try {
                Thread.sleep(5);
            } catch (InterruptedException e) {
            }
        }
    }
    
    //	===================================================================
    
    private int yaw = 0;
    
    private void testModel() {
        
        refdef_t refdef = new refdef_t();
        
        refdef.x = viddef.getWidth() / 2;
        refdef.y = viddef.getHeight() / 2 - 72;
        refdef.width = 144 * 2;
        refdef.height = 168 * 2;
        refdef.fov_x = 40;
        refdef.fov_y = Math3D
        .CalcFov(refdef.fov_x, refdef.width, refdef.height);
        refdef.time = 1.0f * 0.001f;
        
        entity_t entity = new entity_t();
        String modelName = "players/female/tris.md2";
        
        String modelSkin = "players/female/athena.pcx";
        
        String modelImage = "/players/female/athena_i.pcx";
        String modelImage1 = "/players/female/brianna_i.pcx";
        String modelImage2 = "/players/female/cobalt_i.pcx";
        String modelImage3 = "/players/female/lotus_i.pcx";
        
        entity.model = re.RegisterModel(modelName);
        
        drawString(refdef.x, refdef.y - 20, (entity.model != null) ? modelName
                : "DEBUG: NullModel");
        
        entity.skin = re.RegisterSkin(modelSkin);
        entity.flags = Defines.RF_FULLBRIGHT;
        entity.origin[0] = 80;
        entity.origin[1] = 0;
        entity.origin[2] = 0;
        Math3D.VectorCopy(entity.origin, entity.oldorigin);
        entity.frame = (framecount / 3)
        % ((qfiles.dmdl_t) entity.model.extradata).num_frames;
        entity.oldframe = 0;
        entity.backlerp = 0.0f;
        yaw += KBD.mx;
        KBD.mx = 0;
        if (yaw > 360)
            yaw -= 360;
        if (yaw < 0)
            yaw += 360;
        entity.angles[1] = yaw;
        
        refdef.areabits = null;
        refdef.num_entities = 1;
        refdef.entities = new entity_t[] { entity };
        refdef.lightstyles = null;
        refdef.rdflags = Defines.RDF_NOWORLDMODEL;
        
        //			 Menu_Draw(& s_player_config_menu);
        
        M_DrawTextBox((int) ((refdef.x) * (320.0F / viddef.getWidth()) - 8),
                (int) ((viddef.getHeight() / 2) * (240.0F / viddef.getHeight()) - 77),
                refdef.width / 8, refdef.height / 8);
        refdef.height += 4;
        
        re.RenderFrame(refdef);
        
        re.DrawPic(refdef.x - 80, refdef.y, modelImage);
        re.DrawPic(refdef.x - 80, refdef.y + 47, modelImage1);
        re.DrawPic(refdef.x - 80, refdef.y + 94, modelImage2);
        re.DrawPic(refdef.x - 80, refdef.y + 141, modelImage3);
    }
    
    private String[] sprites = { "sprites/s_bfg1.sp2", "sprites/s_bfg2.sp2",
            "sprites/s_bfg3.sp2", "sprites/s_explod.sp2",
            "sprites/s_explo2.sp2", "sprites/s_explo3.sp2",
            "sprites/s_flash.sp2", "sprites/s_bubble.sp2", };
    
    private int spriteCount = 0;
    
    private boolean loading = true;
    
    private void testSprites() {
        
        if (loading) {
            
            re.DrawPic(viddef.getWidth() / 2 - 50, viddef.getHeight() / 2, "loading");
            String name = sprites[spriteCount];
            
            drawString(viddef.getWidth() / 2 - 50, viddef.getHeight() / 2 + 50, name);
            
            re.RegisterModel(name);
            loading = ++spriteCount < sprites.length;
            return;
        }
        
        refdef_t refdef = new refdef_t();
        
        refdef.x = viddef.getWidth() / 2;
        refdef.y = viddef.getHeight() / 2 - 72;
        refdef.width = 144 * 2;
        refdef.height = 168 * 2;
        refdef.fov_x = 40;
        refdef.fov_y = Math3D
        .CalcFov(refdef.fov_x, refdef.width, refdef.height);
        refdef.time = 1.0f * 0.001f;
        
        entity_t entity = new entity_t();
        
        String modelName = sprites[(framecount / 30) % sprites.length];
        drawString(refdef.x, refdef.y - 20, modelName);
        
        entity.model = re.RegisterModel(modelName);
        
        entity.flags = Defines.RF_FULLBRIGHT;
        entity.origin[0] = 80 - (framecount % 200) + 200;
        entity.origin[1] = 0 + (float) (40 * Math.sin(Math
                .toRadians(framecount)));
        entity.origin[2] = 0 + 20;
        Math3D.VectorCopy(entity.origin, entity.oldorigin);
        entity.frame = framecount / 2;
        entity.oldframe = 0;
        entity.backlerp = 0.0f;
        
        refdef.areabits = null;
        refdef.num_entities = 1;
        refdef.entities = new entity_t[] { entity };
        refdef.lightstyles = null;
        refdef.rdflags = Defines.RDF_NOWORLDMODEL;
        
        M_DrawTextBox((int) ((refdef.x) * (320.0F / viddef.getWidth()) - 8),
                (int) ((viddef.getHeight() / 2) * (240.0F / viddef.getHeight()) - 77),
                refdef.width / 8, refdef.height / 8);
        refdef.height += 4;
        
        re.RenderFrame(refdef);
        
    }
    
    private void testBeam() {
        
        refdef_t refdef = new refdef_t();
        
        refdef.x = viddef.getWidth() / 2;
        refdef.y = viddef.getHeight() / 2 - 72;
        refdef.width = 144 * 2;
        refdef.height = 168 * 2;
        refdef.fov_x = 40;
        refdef.fov_y = Math3D
        .CalcFov(refdef.fov_x, refdef.width, refdef.height);
        refdef.time = 1.0f * 0.001f;
        
        entity_t entity = new entity_t();
        
        drawString(refdef.x, refdef.y - 20, "Beam Test");
        
        entity.flags = Defines.RF_BEAM;
        entity.origin[0] = 200;
        entity.origin[1] = 0 + (float) (80 * Math.sin(4 * Math
                .toRadians(framecount)));
        entity.origin[2] = 20 + (float) (40 * Math.cos(4 * Math
                .toRadians(framecount)));
        
        entity.oldorigin[0] = 20;
        entity.oldorigin[1] = 0; // + (float)(40 *
        // Math.sin(Math.toRadians(framecount)));
        entity.oldorigin[2] = -20; // + 20;
        
        entity.frame = 3;
        entity.oldframe = 0;
        entity.backlerp = 0.0f;
        // the four beam colors are encoded in 32 bits of skinnum (hack)
        entity.alpha = 0.6f;
        
        int[] color = { 0xd0, 0xd1, 0xe0, 0xb0 };
        
        entity.skinnum = color[framecount / 2 % 4];
        entity.model = null;
        
        refdef.areabits = null;
        refdef.num_entities = 1;
        refdef.entities = new entity_t[] { entity };
        refdef.lightstyles = null;
        refdef.rdflags = Defines.RDF_NOWORLDMODEL;
        
        M_DrawTextBox((int) ((refdef.x) * (320.0F / viddef.getWidth()) - 8),
                (int) ((viddef.getHeight() / 2) * (240.0F / viddef.getHeight()) - 77),
                refdef.width / 8, refdef.height / 8);
        refdef.height += 4;
        
        re.RenderFrame(refdef);
    }
    
    private LinkedList active_particles = new LinkedList();
    
    private boolean explode = false;
    
    private float[] target;
    
    private void testParticles() {
        
        r_numparticles = 0;
        
        if (active_particles.size() == 0) {
            if (explode)
                Explosion(target);
            else {
                target = new float[] { 150 + Lib.crand() * 80,
                        Lib.crand() * 40, Lib.crand() * 40 };
                RailTrail(new float[] { 30, -20, -20 }, target);
                //Heatbeam(new float[]{30, 20, -20}, target);
            }
            explode = !explode;
        }
        refdef_t refdef = new refdef_t();
        
        refdef.x = viddef.getWidth() / 2;
        refdef.y = viddef.getHeight() / 2 - 72;
        refdef.width = 400;
        refdef.height = 400;
        refdef.fov_x = 50;
        refdef.fov_y = Math3D
        .CalcFov(refdef.fov_x, refdef.width, refdef.height);
        refdef.time = 1.0f * 0.001f;
        
        animateParticles();
        
        drawString(refdef.x, refdef.y - 20, "active particles: "
                + r_numparticles);
        
        refdef.num_particles = r_numparticles;
        
        refdef.areabits = null;
        refdef.num_entities = 0;
        refdef.entities = null;
        refdef.lightstyles = null;
        refdef.rdflags = Defines.RDF_NOWORLDMODEL;
        
        M_DrawTextBox((int) ((refdef.x) * (320.0F / viddef.getWidth()) - 8),
                (int) ((viddef.getHeight() / 2) * (240.0F / viddef.getHeight()) - 77),
                refdef.width / 8, refdef.height / 8);
        refdef.height += 4;
        
        re.RenderFrame(refdef);
    }
    
    private void drawString(int x, int y, String text) {
        for (int i = 0; i < text.length(); i++) {
            re.DrawChar(x + 8 * i, y, (int) text.charAt(i));
        }
    }
    
    private void M_DrawTextBox(int x, int y, int width, int lines) {
        int cx, cy;
        int n;
        
        // draw left side
        cx = x;
        cy = y;
        M_DrawCharacter(cx, cy, 1);
        for (n = 0; n < lines; n++) {
            cy += 8;
            M_DrawCharacter(cx, cy, 4);
        }
        M_DrawCharacter(cx, cy + 8, 7);
        
        // draw middle
        cx += 8;
        while (width > 0) {
            cy = y;
            M_DrawCharacter(cx, cy, 2);
            for (n = 0; n < lines; n++) {
                cy += 8;
                M_DrawCharacter(cx, cy, 5);
            }
            M_DrawCharacter(cx, cy + 8, 8);
            width -= 1;
            cx += 8;
        }
        
        // draw right side
        cy = y;
        M_DrawCharacter(cx, cy, 3);
        for (n = 0; n < lines; n++) {
            cy += 8;
            M_DrawCharacter(cx, cy, 6);
        }
        M_DrawCharacter(cx, cy + 8, 9);
    }
    
    /**
     * M_DrawCharacter
     * 
     * Draws one solid graphics character cx and cy are in 320*240 coordinates,
     * and will be centered on higher res screens. ================
     */
    private void M_DrawCharacter(int cx, int cy, int num) {
        re.DrawChar(cx + ((viddef.getWidth() - 320) >> 1), cy
                + ((viddef.getHeight() - 240) >> 1), num);
    }
    
    long endtime;
    
    private void Explosion(float[] org) {
        cparticle_t p;
        
        for (int i = 0; i < 256; i++) {
            p = new cparticle_t();
            
            p.time = time() * 1.0f;
            p.color = /* 0xe0 */223 - (Lib.rand() & 7);
            for (int j = 0; j < 3; j++) {
                p.org[j] = org[j] + (float) (Lib.rand() % 32) - 16;
                p.vel[j] = (float) (Lib.rand() % 384) - 192;
            }
            
            p.accel[0] = p.accel[1] = 0;
            p.accel[2] = -PARTICLE_GRAVITY;
            p.alpha = 1.0f;
            p.alphavel = -0.8f / (0.5f + Globals.rnd.nextFloat() * 0.3f);
            
            active_particles.add(p);
        }
    }
    
    static final float INSTANT_PARTICLE = -10000.0f;
    
    static final float PARTICLE_GRAVITY = 40.0f;
    
    /*
     * =============== CL_AddParticles ===============
     */
    private void animateParticles() {
        cparticle_t p;
        float alpha;
        float time, time2;
        float[] org = { 0, 0, 0 };
        int color;
        
        time = 0.0f;
        
        for (Iterator it = active_particles.iterator(); it.hasNext();) {
            p = (cparticle_t) it.next();
            
            // PMM - added INSTANT_PARTICLE handling for heat beam
            if (p.alphavel != INSTANT_PARTICLE) {
                time = (time() - p.time) * 0.001f;
                alpha = p.alpha + time * p.alphavel;
                if (alpha <= 0) { // faded out
                    it.remove();
                    continue;
                }
            } else {
                alpha = p.alpha;
            }
            
            if (alpha > 1.0)
                alpha = 1;
            color = (int) p.color;
            
            time2 = time * time;
            
            org[0] = p.org[0] + p.vel[0] * time + p.accel[0] * time2;
            org[1] = p.org[1] + p.vel[1] * time + p.accel[1] * time2;
            org[2] = p.org[2] + p.vel[2] * time + p.accel[2] * time2;
            
            AddParticle(org, color, alpha);
            
            // PMM
            if (p.alphavel == INSTANT_PARTICLE) {
                p.alphavel = 0.0f;
                p.alpha = 0.0f;
            }
        }
    }
    
    private void Heatbeam(float[] start, float[] forward) {
        
        float[] v_up = { 0, 0, 10 };
        float[] v_right = { 0, 10, 0 };
        
        float[] move = { 0, 0, 0 };
        float[] vec = { 0, 0, 0 };
        float len;
        int j;
        cparticle_t p;
        float[] right = { 0, 0, 0 };
        float[] up = { 0, 0, 0 };
        int i;
        float c, s;
        float[] dir = { 0, 0, 0 };
        float ltime;
        float step = 32.0f, rstep;
        float start_pt;
        float rot;
        float variance;
        float[] end = { 0, 0, 0 };
        
        Math3D.VectorMA(start, 4096, forward, end);
        
        Math3D.VectorCopy(start, move);
        Math3D.VectorSubtract(end, start, vec);
        len = Math3D.VectorNormalize(vec);
        
        Math3D.VectorCopy(v_right, right);
        Math3D.VectorCopy(v_up, up);
        //		if (vidref_val == VIDREF_GL)
        //		{ // GL mode
        Math3D.VectorMA(move, -0.5f, right, move);
        Math3D.VectorMA(move, -0.5f, up, move);
        //		}
        //		// otherwise assume SOFT
        
        ltime = (float) time() / 1000.0f;
        start_pt = (ltime * 96.0f) % step;
        Math3D.VectorMA(move, start_pt, vec, move);
        
        Math3D.VectorScale(vec, step, vec);
        
        rstep = (float) Math.PI / 10.0f;
        for (i = (int) start_pt; i < len; i += step) {
            if (i > step * 5) // don't bother after the 5th ring
                break;
            
            for (rot = 0; rot < Math.PI * 2; rot += rstep) {
                
                p = new cparticle_t();
                
                p.time = time();
                Math3D.VectorClear(p.accel);
                variance = 0.5f;
                c = (float) Math.cos(rot) * variance;
                s = (float) Math.sin(rot) * variance;
                
                // trim it so it looks like it's starting at the origin
                if (i < 10) {
                    Math3D.VectorScale(right, c * (i / 10.0f), dir);
                    Math3D.VectorMA(dir, s * (i / 10.0f), up, dir);
                } else {
                    Math3D.VectorScale(right, c, dir);
                    Math3D.VectorMA(dir, s, up, dir);
                }
                
                p.alpha = 0.8f;
                p.alphavel = -1000.0f;
                p.color = /* 223 */0x74 - (Lib.rand() & 7);
                for (j = 0; j < 3; j++) {
                    p.org[j] = move[j] + dir[j] * 3;
                    p.vel[j] = 0;
                }
                
                active_particles.add(p);
            }
            Math3D.VectorAdd(move, vec, move);
        }
    }
    
    private void RailTrail(float[] start, float[] end) {
        float[] move = { 0, 0, 0 };
        float[] vec = { 0, 0, 0 };
        float len;
        int j;
        cparticle_t p;
        float dec;
        float[] right = { 0, 0, 0 };
        float[] up = { 0, 0, 0 };
        int i;
        float d, c, s;
        float[] dir = { 0, 0, 0 };
        
        Math3D.VectorCopy(start, move);
        Math3D.VectorSubtract(end, start, vec);
        len = Math3D.VectorNormalize(vec);
        
        Math3D.MakeNormalVectors(vec, right, up);
        
        for (i = 0; i < len; i++) {
            
            p = new cparticle_t();
            p.time = time();
            Math3D.VectorClear(p.accel);
            
            d = i * 0.1f;
            c = (float) Math.cos(d);
            s = (float) Math.sin(d);
            
            Math3D.VectorScale(right, c, dir);
            Math3D.VectorMA(dir, s, up, dir);
            
            p.alpha = 1.0f;
            p.alphavel = -1.0f / (1 + Globals.rnd.nextFloat() * 0.2f);
            p.color = 0x74 + (Lib.rand() & 7);
            for (j = 0; j < 3; j++) {
                p.org[j] = move[j] + dir[j] * 3;
                p.vel[j] = dir[j] * 6;
            }
            
            Math3D.VectorAdd(move, vec, move);
            
            active_particles.add(p);
        }
        
        dec = 0.75f;
        Math3D.VectorScale(vec, dec, vec);
        Math3D.VectorCopy(start, move);
        
        while (len > 0) {
            len -= dec;
            
            p = new cparticle_t();
            
            p.time = time();
            Math3D.VectorClear(p.accel);
            
            p.alpha = 1.0f;
            p.alphavel = -1.0f / (0.6f + Globals.rnd.nextFloat() * 0.2f);
            p.color = 0x0 + Lib.rand() & 15;
            
            for (j = 0; j < 3; j++) {
                p.org[j] = move[j] + Lib.crand() * 3;
                p.vel[j] = Lib.crand() * 3;
                p.accel[j] = 0;
            }
            
            Math3D.VectorAdd(move, vec, move);
            active_particles.add(p);
        }
    }
    
    private int time() {
        return (int) (System.currentTimeMillis() - startTime);
    }
    
    static xcommand_t nexttest = new xcommand_t() {
        public void execute() {
            testnr++;
            testnr = testnr % 3;
        }
    };
    
    int r_numparticles = 0;
    
    /**
     * V_AddParticle
     */
    void AddParticle(float[] org, int color, float alpha) {
        if (r_numparticles >= Defines.MAX_PARTICLES)
            return;
        
        int i = r_numparticles++;
        
        int c = particle_t.colorTable[color];
        c |= (int) (alpha * 255) << 24;
        particle_t.colorArray.put(i, c);
        
        i *= 3;
        FloatBuffer vertexBuf = particle_t.vertexArray;
        vertexBuf.put(i, org[0]);
        vertexBuf.put(i + 1, org[1]);
        vertexBuf.put(i + 2, org[2]);
    }
}