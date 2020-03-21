import { Component, OnInit } from '@angular/core';
import { Message } from '../_models/message';
import { Pagination, PaginatedResult } from '../_models/Pagination';
import { AuthService } from '../_services/auth.service';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
messages: Message[];
pagination: Pagination;
messageContainer: 'Unread';

  constructor(private authService: AuthService, private userService: UserService, 
              private alertify: AlertifyService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.messages = data['users'].result;
      this.pagination = data['users'].pagination;
    });
  }
  pageChanged(event: any): void{
    this.pagination.currentPage = event.page;
    this.loadMessages();
  }
  
  deleteMessage(id: number){
   this.alertify.confirm('Are you sure you want to delete this message',() => {
     this.userService.deleteMessage(id, this.authService.decodedToken.nameid).subscribe(() =>{
        this.messages.splice(this.messages.findIndex(c => c.id === id), 1);
        this.alertify.success('Message has been deleted');
     }, error => {
      this.alertify.success('Failed to delete the message');
     });
   });
  }

  loadMessages() {
    this.userService.getMessages(this.authService.decodedToken.nameid, this.pagination.currentPage, 
              this.pagination.itemsPerPage, this.messageContainer)
            .subscribe((res: PaginatedResult<Message[]>) => {
              this.messages = res.result;
              this.pagination = res.pagination;
            }, error => {
              this.alertify.error(error);
            });
  }
}
 