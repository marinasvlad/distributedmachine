import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ImagesService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  process(){
    return this.http.get(this.baseUrl + "images", {responseType: 'text'});
  }  

  onSubmit(selectedFile: File){
    if (!selectedFile) return;

    const formData = new FormData();
    formData.append('file', selectedFile, selectedFile.name);

    this.http.post(this.baseUrl + "images", formData).subscribe({
      next: (response) => {

      },
      error: (err) => {

      },
    });
  }  
  
}
