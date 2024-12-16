import { Component, OnInit } from '@angular/core';
import { AccountService } from '../services/account.service';
import { ImagesService } from '../services/images.service';
import { ImageObject } from '../models/image';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-process',
  templateUrl: './process.component.html',
  styleUrls: ['./process.component.css']
})
export class ProcessComponent implements OnInit {
  images: ImageObject[] = [];
  selectedFile: File | null = null;
  constructor(private imagesService: ImagesService, private http: HttpClient) { }
  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
    }
  }
  ngOnInit(): void {
    this.http.get<ImageObject[]>('https://localhost:5006/api/images')  // Adjust URL as necessary
      .subscribe(
        (response) => {
          this.images = response;
        },
        (error) => {
          console.error('Error fetching images', error);
        }
      );
  }
  process(){
    this.imagesService.process().subscribe(() =>
    {
    }
    );
  }  
  onSubmit(): void {
    if (!this.selectedFile) return;

    this.imagesService.onSubmit(this.selectedFile);
  }

  private byteArrayToBase64(byteArray: Uint8Array): string {
    return `data:image/jpeg;base64,${btoa(String.fromCharCode(...byteArray))}`;
  }
}
