import { Injectable } from '@angular/core';
import { Observable, from } from 'rxjs';
import { HttpClient } from '@angular/common/http'
import { environment } from '../../environments/environment'

@Injectable({
  providedIn: 'root'
})
export class SwfileService {

  //构造器 注入 httpclient
  constructor(private http:HttpClient) { }

  //上传SolidWorks文件
  postFile(fileToUpload: File): any {
    const endpoint =environment.serviceUrl;//serviceUrl+"SWFile";
    const formData: FormData = new FormData();
    formData.append('fileKey', fileToUpload, fileToUpload.name);
    return this.http.post(endpoint,formData,null);
  }
  
}
