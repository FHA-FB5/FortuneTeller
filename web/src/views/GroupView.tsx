import * as React from "react";

interface GroupViewProps {
  groupName: string
}

export class GroupView<P extends GroupViewProps> extends React.Component<P, {}> {
  render() {
    return (
      <div className="container center">
        <p className="medium">Du bist in der Gruppe</p>
        <h1 className="teal-text text-accent-4">{this.props.groupName}</h1>
      </div>
    )
  }
}
