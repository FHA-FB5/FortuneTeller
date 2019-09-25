import * as React from "react";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faGithub} from "@fortawesome/free-brands-svg-icons";
import {faStar} from "@fortawesome/free-solid-svg-icons";
import Wave from "./components/wave/Wave";
import {RegisterView} from "./views/RegisterView";
import "./config"
import {config} from "./config";
import "./App.css"
import {Group} from "./api/model";
import {GroupView} from "./views/GroupView";

interface AppState {
  isRegistered: boolean
  registrationFailed: boolean
  groupName: string
}

class App<T extends AppState> extends React.Component<{}, T> {
  constructor() {
    super({})
    this.state = {
      isRegistered: false
    } as T
  }

  render() {
    return (
      <div className="App">
        <div className="row teal accent-4" style={{height: "20vh", margin: 0}}>
          <div className="col s12">
            <h1 className="show-on-large white-text center"
                style={{display: "none", whiteSpace: "nowrap", fontFamily: "'Parisienne'", fontSize: "5em"}}>
              {config.projectName}
            </h1>
            <h1 className="show-on-medium-and-down white-text center"
                style={{display: "none", whiteSpace: "nowrap", fontFamily: "'Parisienne'", fontSize: "3em"}}>
              Fortune Teller
            </h1>
          </div>
        </div>
        <div className="row" style={{height: "5vw", margin: 0}}>
          <Wave colorBottom="white" colorTop="#00bfa5" count={1} maxWaves={1} rotate={false}/>
        </div>
        <div className="body white row valign-wrapper">
          {this.state.isRegistered
            ?
            <GroupView groupName={this.state.groupName}/>
            :
            <RegisterView onFailure={() => this.setState({registrationFailed: true})} onSuccess={response => {
              fetch(config.apiUrl + "/group/" + response.groupId, {
                method: 'get'
              }).then(response => {
                if (response.status !== 200) {
                  throw Error(`invalid status: ${response.status} ${response.statusText}`)
                }
                return response.json()
              }).then(group => {
                console.log((group as Group).id)
                this.setState({
                  isRegistered: true,
                  groupName: (group as Group).name,
                })
              })
            }}/>
          }
        </div>
        <div style={{height: "5vw", margin: 0}}>
          <Wave colorBottom="#00bfa5" colorTop="white" count={1} maxWaves={1} rotate={true}/>
        </div>
        <footer className="row page-footer teal accent-4" style={{height: "10vh", margin: 0}}>
          <div className="footer-copyright">
            <div className="container">
              © 2019 <a href={config.copyrightUrl} className="grey-text text-lighten-2">{config.copyrightOwnerShort}</a>
              <div className="right">
                <a href={config.sourceUrl} className="grey-text text-lighten-2">
                  <FontAwesomeIcon icon={faStar}/> me on <FontAwesomeIcon icon={faGithub}/>
                </a>
              </div>
            </div>
          </div>
        </footer>
      </div>
    )
  }
}

export default App;
