import {
    Scene,
    AmbientLight,
    PointLight,
    WebGLRenderer,
    PerspectiveCamera,
    GridHelper,
    Color
 } from 'three';
//  import {
//     OrbitControls,
//  } from 'three-full';
 import * as Stats from 'stats.js';
 import * as TWEEN from '@tweenjs/tween.js';
 import {
  FXAAShader, UnrealBloomPass, ShaderPass, FilmPass, OutlinePass, GeometryUtils, CopyShader, ColorifyShader, SepiaShader,
  OrbitControls, GLTFLoader, EffectComposer, RenderPass, SMAAShader, SMAAPass, ClearMaskPass, MaskPass,
 } from 'three-full';
 
 // three-full和tween以下代码没调用，先展示下调用方式
 
 
 //  渲染器
 export const RENDERER = new WebGLRenderer(); //  渲染器(去据此){ antialias: true }
 export function initRenderer(doc) {
    RENDERER.setSize(
        doc.clientWidth,
        doc.clientHeight
    );
    RENDERER.shadowMap.enabled = true; // 辅助线
    doc.appendChild(RENDERER.domElement);
 }
 
 
 // 场景
 export const SCENE = new Scene();
 export function initScene() {
    SCENE.background = new Color(255,255,255);//new Color(0xcccccc);
 }
 
 //  灯光
 export function initLight() {
    const ambientLight = new AmbientLight(0xffffff, 0.2);    // 全局光
    ambientLight.position.set(10, 20, 55);   // 灯光
    SCENE.add(ambientLight);
 
    // 点光源
    const pointLight = new PointLight(0xffffff);
    pointLight.distance = 0;
    CAMERA.add(pointLight);
    SCENE.add(CAMERA);
 }
 
 //  相机
 export let CAMERA;
 export let CONTROLS;
 export function initCamera(doc) {
    const d = {
        fov: 1, // 拍摄距离  视野角值越大，场景中的物体越小
        near: 0.1, //  最小范围
        far: 1000, //  最大范围
    };
    CAMERA = new PerspectiveCamera(
        d.fov,
        doc.clientWidth / doc.clientHeight,
        d.near,
        d.far)
        ;
    const p = {
        x: -20,
        y: 10,
        z: -10,
    };
    CAMERA.position.set(p.x, p.y, p.z);
    CAMERA.lookAt(0, 0, 0);
    CONTROLS = new OrbitControls(CAMERA, RENDERER.domElement);  // 控制镜头
 }
 
 
 //  网格
 export function initGrid() {
    const gridHelper = new GridHelper(100, 50);
    SCENE.add(gridHelper);
 }
 
 
 //  性能检测
 export const STATS = new Stats();
 export function initStats(doc) {
    STATS.setMode(0);
    STATS.domElement.style.position = 'absolute';
    STATS.domElement.left = '0px';
    STATS.domElement.top = '0px';
    doc.appendChild(STATS.domElement);
 }
 
 //  动画混合器组（把模型的动画混合器都push到这里面，在canvas.ts里面更新动画   ）
 export const MIXER = [];