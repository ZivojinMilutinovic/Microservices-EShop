import { Component, Input, OnInit, Output } from '@angular/core';
import { EventEmitter } from '@angular/core';

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.scss']
})
export class UploadComponent implements OnInit {
  public progress: number;
  public message: string;

   @Input() public uploadMessage:string="Upload file";
  @Output() public onUploadFinished = new EventEmitter();
  constructor() { }

  ngOnInit(): void {
  }
  public uploadFile=(files:any)=>{
    if(files.length==0){
      return;
    }
    let fileToUpload:any=<File>files[0];
    const formData=new FormData();
    formData.append('file',fileToUpload,fileToUpload.name);
    this.onUploadFinished.emit(fileToUpload);

  }

}
