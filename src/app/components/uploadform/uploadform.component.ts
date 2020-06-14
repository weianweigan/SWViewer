import { Component, OnInit, Injectable,ViewChild, ElementRef} from '@angular/core';
import { Observable, from } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { SwfileService } from '../../services/swfile.service'

import {
  initRenderer,
  initCamera,
  initScene, initLight,
  initGrid, initStats,
  RENDERER, CAMERA, SCENE, CONTROLS, STATS, MIXER
} from '../../services/base';
import {
  FXAAShader, UnrealBloomPass, ShaderPass, FilmPass, OutlinePass, GeometryUtils, CopyShader, ColorifyShader, SepiaShader,
  OrbitControls, GLTFLoader, EffectComposer, RenderPass, SMAAShader, SMAAPass, ClearMaskPass, MaskPass,
} from 'three-full';
import * as  TWEEN from '@tweenjs/tween.js';
import { Vector2, Group, Scene, SphereGeometry, ImageUtils, AnimationMixer } from 'three';
import { fromEvent } from 'rxjs';
import * as THREE from 'three';

@Component({
  selector: 'app-uploadform',
  templateUrl: './uploadform.component.html',
  styleUrls: ['./uploadform.component.css']
})

export class UploadformComponent implements OnInit {
  
  @ViewChild('canvasFrame', { static: true }) canvasContainer: ElementRef;
  
  thing;

  constructor(private swfile:SwfileService) { }

  public fileToUpload:File = null;

  ngOnInit(): void {
    this.init();
  }

  handleFileInput(files:FileList)
  {
    this.fileToUpload = files.item(0);

    alert(this.fileToUpload.name);
    
    //上传文件
    //this.swfile.postFile(this.fileToUpload);
  }

init()
{
  initRenderer(this.canvasContainer.nativeElement);
  initCamera(this.canvasContainer.nativeElement);
  initScene();
  initLight();
  //initGrid();
  initStats(this.canvasContainer.nativeElement);

   //  加载模型-star
   this.importantModel();
   //  加载模型-end


   //  渲染场景
   //const delta = new Clock();
   const rendererOut = () => {
     
     requestAnimationFrame(rendererOut);
     RENDERER.render(SCENE, CAMERA);
     CONTROLS.update();
     STATS.update();
      if (MIXER) {
            MIXER.map(r => {
              r.update(5);
            });
          }
   };

   rendererOut();
}

// 这个模型可以使用blender2.8(正处于beta版) 直接导出gltf
importantModel() {
  const loader = new GLTFLoader();

    loader.load( 'assets/models/flangebolt.gltf', function ( gltf ) {
      SCENE.add( gltf.scene );

      gltf.scene.traverse((child) =>
      {
        if (child.isMesh) {
                console.log(child);
                child.material.color =new THREE.Color("rgb(112, 128, 144)"); //  颜色
                child.material.metalness = 0.8;   //  金属度
                //gltf.scene.background = 'rgba(0,0,0, 0.5)';
                //gltf.scene.translateX(5);
                //this.thing = gltf.scene;
                //SCENE.add(this.thing);
        }
      }
      );

    }, undefined, function ( error ) {
    
      console.error( error );
    
    } );

  // loader.load('assets/models/flangebolt.glb', (gltf) => {
  //   console.log(gltf);
  //   gltf.scene.traverse((child) => {  // 遍历判断Mesh
  //     if (child.isMesh) {
  //       console.log(child);
  //       child.material.color = { r: 1, g: 2, b: 3 };    //  颜色
  //       child.material.metalness = 0.8;   //  金属度
  //       gltf.scene.background = 'rgba(0,0,0, 0.5)';
  //       gltf.scene.translateX(5);
  //       this.thing = gltf.scene;
  //       SCENE.add(this.thing);
  //     }
  //   });
  // },
  //   undefined,
  //   (error) => {
  //     console.error(error);
  //   });
}

}
