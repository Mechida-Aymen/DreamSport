import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PutObjectCommand, S3Client } from '@aws-sdk/client-s3';

@Injectable({
  providedIn: 'root'
})
export class CloudflareService {
  // L'URL de ton bucket Cloudflare R2
  private bucketUrl = 'https://e3e3b386259734acfcc14178a94ae982.r2.cloudflarestorage.com'; // Remplace par ton URL R2
  private accessKey = '6bf5ee7ffccf0841651dee0e9b6be0ca'; // Remplace par ta clé d'accès
  private secretKey = 'a8ffa9a85b1d693620de19424e35edd692642e1e0c8c18e170ffb100d841e30b'; // Remplace par ta clé secrète
  private s3Client: S3Client;  // Déclaration explicite

  constructor(private http: HttpClient  
  ) {

    this.s3Client = new S3Client({  
      region: 'auto',
      endpoint: this.bucketUrl,
      credentials: {
        accessKeyId: this.accessKey,  // Remplacez par vos clés
        secretAccessKey: this.secretKey
      }
    });

  }

 
  async uploadFile(file: File): Promise<string> {
    const bucketName = 'dreamsport-saas'; 
    const fileBuffer = await file.arrayBuffer();
    const uint8Array = new Uint8Array(fileBuffer);
  
    const command = new PutObjectCommand({
      Bucket: bucketName,
      Key: file.name,
      Body: uint8Array,
      ContentType: file.type,
      ACL: 'public-read' 
    });
  
    try {
      await this.s3Client.send(command);
      return `https://pub-ae615910610b409dbb3d91c073aa47e6.r2.dev/${encodeURIComponent(file.name)}`;
    } catch (error) {
      console.error('Upload error:', error);
      throw error;
    }
  }


  
  getImage(imageUrl: string): Observable<Blob> {
    return this.http.get(imageUrl, { responseType: 'blob' });
  }
}
